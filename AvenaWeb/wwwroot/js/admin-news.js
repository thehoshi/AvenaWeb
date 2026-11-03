(() => {
    const form = document.getElementById("newsEditor");
    if (!form) return;

    const titleInput = document.getElementById("newsTitle");
    const slugInput = document.getElementById("newsSlug");
    const message = document.getElementById("editorMessage");
    let slugWasEdited = Boolean(slugInput.value);

    const slugify = (value) => {
        const slug = value
            .normalize("NFKC")
            .toLowerCase()
            .replace(/[^\p{L}\p{N}]+/gu, "-")
            .replace(/^-+|-+$/g, "");
        if (!slug) return "article";
        const nonNumericSlug = /^\p{N}+$/u.test(slug) ? `article-${slug}` : slug;
        return nonNumericSlug.slice(0, 180).replace(/-+$/g, "");
    };

    titleInput.addEventListener("input", () => {
        if (!slugWasEdited) slugInput.value = slugify(titleInput.value);
    });
    slugInput.addEventListener("input", () => {
        slugWasEdited = true;
    });

    const readResponse = async (response) => {
        const result = await response.json().catch(() => null);
        if (!response.ok) {
            if (response.status === 401 || response.status === 403) {
                throw new Error("Your admin session is no longer valid. Sign in again and retry.");
            }
            const detail = result?.error || result?.title || "The article could not be saved.";
            throw new Error(detail);
        }
        return result;
    };

    const send = async (url, method, body, token) => {
        const response = await fetch(url, {
            method,
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                "X-CSRF-TOKEN": token
            },
            body: body === undefined ? undefined : JSON.stringify(body)
        });
        return readResponse(response);
    };

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        const action = event.submitter?.dataset.action || "save";
        const token = form.querySelector('input[name="__RequestVerificationToken"]').value;
        const buttons = [...form.querySelectorAll("button")];
        const payload = {
            title: document.getElementById("newsTitle").value.trim(),
            slug: slugInput.value.trim(),
            summary: document.getElementById("newsSummary").value.trim(),
            bodyMarkdown: document.getElementById("newsBody").value.trim(),
            genreId: Number(document.getElementById("newsGenre").value),
            imageUrl: document.getElementById("newsImage").value.trim(),
            authorId: Number(document.getElementById("newsAuthor").value) || null
        };

        message.hidden = true;
        buttons.forEach((button) => { button.disabled = true; });

        try {
            let articleId = form.dataset.newsId ? Number(form.dataset.newsId) : null;
            const saved = await send(
                articleId ? `/api/admin/news/${articleId}` : "/api/admin/news",
                articleId ? "PUT" : "POST",
                payload,
                token
            );

            articleId = articleId || saved.id;
            form.dataset.newsId = String(articleId);
            if (saved.slug) slugInput.value = saved.slug;

            if (action === "publish") {
                await send(`/api/admin/news/${articleId}/publish`, "POST", undefined, token);
            }

            window.location.assign("/Admin/News");
        } catch (error) {
            message.textContent = error instanceof Error ? error.message : "The article could not be saved.";
            message.classList.add("error");
            message.hidden = false;
        } finally {
            buttons.forEach((button) => { button.disabled = false; });
        }
    });
})();
