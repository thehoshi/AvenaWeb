(() => {
    const form = document.getElementById("newsEditor");
    if (!form) return;

    const root = document.documentElement;
    const titleInput = document.getElementById("newsTitle");
    const slugInput = document.getElementById("newsSlug");
    const message = document.getElementById("editorMessage");
    const imageInput = document.getElementById("newsImage");
    const imageFile = document.getElementById("newsImageFile");
    const imagePreview = document.getElementById("imagePreview");
    const imageMessage = document.getElementById("imageUploadMessage");
    const topicSelect = document.getElementById("newsGenre");
    const topicCreator = document.getElementById("topicCreator");
    const topicCreatorDetails = document.getElementById("topicCreatorDetails");
    const newTopicName = document.getElementById("newTopicName");
    const token = form.querySelector('input[name="__RequestVerificationToken"]').value;
    let slugWasEdited = Boolean(slugInput.value);

    const slugify = (value) => {
        const slug = value.normalize("NFKC").toLowerCase()
            .replace(/[^\p{L}\p{N}]+/gu, "-").replace(/^-+|-+$/g, "");
        if (!slug) return "article";
        const nonNumericSlug = /^\p{N}+$/u.test(slug) ? `article-${slug}` : slug;
        return nonNumericSlug.slice(0, 180).replace(/-+$/g, "");
    };

    const messageText = (key) => {
        const language = root.dataset.language || "en";
        const messages = window.avenaAdminMessages || {};
        return messages[language]?.[key] || messages.en?.[key] || key;
    };

    const translateError = (value) => {
        const errors = {
            "A title and article body are required.": "required",
            "A title and article body are required before publishing.": "required",
            "The slug may contain letters, numbers, and hyphens only.": "slug",
            "That article slug is already in use. Choose a different slug.": "duplicateSlug",
            "The article slug is invalid.": "slug",
            "Choose a valid topic.": "chooseTopic",
            "Choose a valid topic before publishing.": "chooseTopic",
            "Use an HTTPS image URL or a path inside /images/.": "imagePath",
            "A topic name must be between 2 and 80 characters.": "topicLength",
            "The byline must be 160 characters or fewer.": "bylineLength",
            "Choose an image file first.": "chooseImage",
            "Images must be 5 MB or smaller.": "imageSize",
            "Use a valid JPG, PNG, or WebP image.": "imageType"
        };
        return errors[value] ? messageText(errors[value]) : value;
    };

    const showMessage = (text) => {
        message.textContent = text;
        message.classList.add("error");
        message.hidden = false;
    };

    const readResponse = async (response) => {
        const result = await response.json().catch(() => null);
        if (!response.ok) {
            if (response.status === 401 || response.status === 403)
                throw new Error(messageText("session"));
            throw new Error(translateError(result?.error || result?.title || messageText("saveFailed")));
        }
        return result;
    };

    const send = async (url, method, body) => readResponse(await fetch(url, {
        method,
        credentials: "same-origin",
        headers: { "Content-Type": "application/json", "X-CSRF-TOKEN": token },
        body: body === undefined ? undefined : JSON.stringify(body)
    }));

    titleInput.addEventListener("input", () => { if (!slugWasEdited) slugInput.value = slugify(titleInput.value); });
    slugInput.addEventListener("input", () => { slugWasEdited = true; });

    topicCreatorDetails?.addEventListener("toggle", () => {
        if (topicCreatorDetails.open) newTopicName.focus();
    });

    document.getElementById("createTopic")?.addEventListener("click", async (event) => {
        const button = event.currentTarget;
        const name = newTopicName.value.trim();
        if (!name) { newTopicName.focus(); return; }
        button.disabled = true;
        message.hidden = true;
        try {
            const created = await send("/api/admin/news/topics", "POST", { name });
            let option = [...topicSelect.options].find(item => Number(item.value) === created.id);
            if (!option) {
                option = new Option(created.name, created.id);
                topicSelect.add(option);
            }
            topicSelect.value = String(created.id);
            newTopicName.value = "";
            topicCreatorDetails.open = false;
        } catch (error) {
            showMessage(error instanceof Error ? error.message : messageText("topicFailed"));
        } finally {
            button.disabled = false;
        }
    });

    imageFile?.addEventListener("change", async () => {
        const file = imageFile.files?.[0];
        if (!file) return;
        const formData = new FormData();
        formData.append("file", file);
        imageFile.disabled = true;
        imageMessage.textContent = messageText("uploading");
        message.hidden = true;
        try {
            const response = await fetch("/api/admin/news/image", {
                method: "POST",
                credentials: "same-origin",
                headers: { "X-CSRF-TOKEN": token },
                body: formData
            });
            const uploaded = await readResponse(response);
            imageInput.value = uploaded.imageUrl;
            imagePreview.src = uploaded.imageUrl;
            imagePreview.hidden = false;
            imageMessage.textContent = messageText("uploaded");
        } catch (error) {
            imageFile.value = "";
            imageMessage.textContent = "";
            showMessage(error instanceof Error ? error.message : messageText("uploadFailed"));
        } finally {
            imageFile.disabled = false;
        }
    });

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        const action = event.submitter?.dataset.action || "save";
        const buttons = [...form.querySelectorAll("button")];
        const payload = {
            title: titleInput.value.trim(),
            slug: slugInput.value.trim(),
            summary: document.getElementById("newsSummary").value.trim(),
            bodyMarkdown: document.getElementById("newsBody").value.trim(),
            genreId: Number(topicSelect.value),
            imageUrl: imageInput.value.trim(),
            authorName: document.getElementById("newsAuthor").value.trim() || null
        };
        message.hidden = true;
        buttons.forEach(button => { button.disabled = true; });
        try {
            let id = form.dataset.newsId ? Number(form.dataset.newsId) : null;
            const saved = await send(id ? `/api/admin/news/${id}` : "/api/admin/news", id ? "PUT" : "POST", payload);
            id = id || saved.id;
            form.dataset.newsId = String(id);
            if (saved.slug) slugInput.value = saved.slug;
            if (action === "publish") await send(`/api/admin/news/${id}/publish`, "POST", undefined);
            window.location.assign("/Admin/News");
        } catch (error) {
            showMessage(error instanceof Error ? error.message : messageText("saveFailed"));
        } finally {
            buttons.forEach(button => { button.disabled = false; });
        }
    });
})();
