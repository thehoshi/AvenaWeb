(() => {
    const root = document.documentElement;
    const themeToggle = document.getElementById("themeToggle");
    const languageSelect = document.getElementById("languageSelect");
    const navToggle = document.getElementById("navToggle");
    const navLinks = document.getElementById("navLinks");

    const translations = {
        en: {
            "home.est_2026_an_open_forum": "Est. 2026 \u2014 An open forum",
            "home.opinions_shape_the_future": "Opinions shape the future.",
            "home.stay_informed_stay_engaged": "Stay informed. Stay engaged.",
            "home.read_latest_news": "Read Latest News",
            "home.about_the_project": "About the Project",
            "home.why_this_platform_exists": "Why this platform exists",
            "home.who_created_it": "Who created it",
            "home.mission": "Mission",
            "home.purpose": "Purpose",
            "home.year": "Year",
            "home.latest_news": "Latest News",
            "home.what_people_are_talking_about": "What people are talking about",
            "home.no_articles_have_been_published_yet_check_back_soon": "No articles have been published yet. Check back soon.",
            "home.browse_by_topic": "Browse by Topic",
            "home.no_topics_yet": "No topics yet.",
            "home.contact": "Contact",
            "home.let_s_talk": "Let's talk",
            "nav.home":"Home", "nav.news":"News", "nav.profile":"Profile", "nav.login":"Login", "nav.logout":"Logout",
            "footer.copy":"Avena © 2026", "footer.privacy":"Privacy Policy", "footer.terms":"Terms",
            "common.or":"or", "form.username":"Username", "form.password":"Password", "form.name":"Name",
            "login.welcome":"Welcome back", "login.subtitle":"Log in to continue to Avena", "login.button":"Login",
            "register.create":"Create an account", "register.heading":"New here? Let's make you an Avena account.", "register.button":"Create account", "register.success":"Account created. You are now logged in."
        },
        ru: {
            "home.est_2026_an_open_forum": "Est. 2026 \u2014 \u041e\u0442\u043a\u0440\u044b\u0442\u044b\u0439 \u0444\u043e\u0440\u0443\u043c",
            "home.opinions_shape_the_future": "\u041c\u043d\u0435\u043d\u0438\u044f \u0444\u043e\u0440\u043c\u0438\u0440\u0443\u044e\u0442 \u0431\u0443\u0434\u0443\u0449\u0435\u0435.",
            "home.stay_informed_stay_engaged": "\u0411\u0443\u0434\u044c\u0442\u0435 \u0432 \u043a\u0443\u0440\u0441\u0435. \u0423\u0447\u0430\u0441\u0442\u0432\u0443\u0439\u0442\u0435.",
            "home.read_latest_news": "\u0427\u0438\u0442\u0430\u0442\u044c \u043f\u043e\u0441\u043b\u0435\u0434\u043d\u0438\u0435 \u043d\u043e\u0432\u043e\u0441\u0442\u0438",
            "home.about_the_project": "\u041e \u043f\u0440\u043e\u0435\u043a\u0442\u0435",
            "home.why_this_platform_exists": "\u0417\u0430\u0447\u0435\u043c \u0441\u0443\u0449\u0435\u0441\u0442\u0432\u0443\u0435\u0442 \u044d\u0442\u0430 \u043f\u043b\u0430\u0442\u0444\u043e\u0440\u043c\u0430",
            "home.who_created_it": "\u041a\u0442\u043e \u0435\u0451 \u0441\u043e\u0437\u0434\u0430\u043b",
            "home.mission": "\u041c\u0438\u0441\u0441\u0438\u044f",
            "home.purpose": "\u041d\u0430\u0437\u043d\u0430\u0447\u0435\u043d\u0438\u0435",
            "home.year": "\u0413\u043e\u0434",
            "home.latest_news": "\u041f\u043e\u0441\u043b\u0435\u0434\u043d\u0438\u0435 \u043d\u043e\u0432\u043e\u0441\u0442\u0438",
            "home.what_people_are_talking_about": "\u041e \u0447\u0451\u043c \u0433\u043e\u0432\u043e\u0440\u044f\u0442",
            "home.no_articles_have_been_published_yet_check_back_soon": "\u0421\u0442\u0430\u0442\u0435\u0439 \u043f\u043e\u043a\u0430 \u043d\u0435\u0442. \u0417\u0430\u0433\u043b\u044f\u043d\u0438\u0442\u0435 \u043f\u043e\u0437\u0436\u0435.",
            "home.browse_by_topic": "\u041f\u043e \u0442\u0435\u043c\u0430\u043c",
            "home.no_topics_yet": "\u041f\u043e\u043a\u0430 \u043d\u0435\u0442 \u0442\u0435\u043c.",
            "home.contact": "\u041a\u043e\u043d\u0442\u0430\u043a\u0442\u044b",
            "home.let_s_talk": "\u0414\u0430\u0432\u0430\u0439\u0442\u0435 \u043f\u043e\u0433\u043e\u0432\u043e\u0440\u0438\u043c",
            "nav.home":"Главная", "nav.news":"Новости", "nav.profile":"Профиль", "nav.login":"Войти", "nav.logout":"Выйти",
            "footer.copy":"Avena © 2026", "footer.privacy":"Политика конфиденциальности", "footer.terms":"Условия",
            "common.or":"или", "form.username":"Имя пользователя", "form.password":"Пароль", "form.name":"Имя",
            "login.welcome":"С возвращением", "login.subtitle":"Войдите, чтобы продолжить в Avena", "login.button":"Войти",
            "register.create":"Создать аккаунт", "register.heading":"Впервые здесь? Давайте создадим аккаунт Avena.", "register.button":"Создать аккаунт", "register.success":"Аккаунт создан. Вы вошли в систему."
        },
        az: {
            "home.est_2026_an_open_forum": "Est. 2026 \u2014 A\u00e7\u0131q forum",
            "home.opinions_shape_the_future": "Fikirl\u0259r g\u0259l\u0259c\u0259yi formala\u015fd\u0131r\u0131r.",
            "home.stay_informed_stay_engaged": "M\u0259lumatl\u0131 olun. F\u0259al olun.",
            "home.read_latest_news": "Son x\u0259b\u0259rl\u0259ri oxu",
            "home.about_the_project": "Layih\u0259 haqq\u0131nda",
            "home.why_this_platform_exists": "Bu platforma niy\u0259 m\u00f6vcuddur",
            "home.who_created_it": "Kim yarad\u0131b",
            "home.mission": "Missiya",
            "home.purpose": "M\u0259qs\u0259d",
            "home.year": "\u0130l",
            "home.latest_news": "Son x\u0259b\u0259rl\u0259r",
            "home.what_people_are_talking_about": "\u0130nsanlar\u0131n dan\u0131\u015fd\u0131qlar\u0131",
            "home.no_articles_have_been_published_yet_check_back_soon": "H\u0259l\u0259 m\u0259qal\u0259 d\u0259rc edilm\u0259yib. Sonra yenid\u0259n bax\u0131n.",
            "home.browse_by_topic": "M\u00f6vzulara g\u00f6r\u0259 bax",
            "home.no_topics_yet": "H\u0259l\u0259 m\u00f6vzu yoxdur.",
            "home.contact": "\u018flaq\u0259",
            "home.let_s_talk": "G\u0259lin dan\u0131\u015faq",
            "nav.home":"Ana səhifə", "nav.news":"Xəbərlər", "nav.profile":"Profil", "nav.login":"Daxil ol", "nav.logout":"Çıxış",
            "footer.copy":"Avena © 2026", "footer.privacy":"Məxfilik siyasəti", "footer.terms":"Şərtlər",
            "common.or":"və ya", "form.username":"İstifadəçi adı", "form.password":"Şifrə", "form.name":"Ad",
            "login.welcome":"Xoş gəldiniz", "login.subtitle":"Avenaya davam etmək üçün daxil olun", "login.button":"Daxil ol",
            "register.create":"Hesab yarat", "register.heading":"Burada yenisiniz? Gəlin Avena hesabı yaradaq.", "register.button":"Hesab yarat", "register.success":"Hesab yaradıldı. Sistemə daxil oldunuz."
        }
    };

    function applyTheme(theme) {
        root.dataset.theme = theme;
        localStorage.setItem("avena-theme", theme);
        if (themeToggle) themeToggle.textContent = theme === "dark" ? "☀" : "☾";
        if (themeToggle) themeToggle.setAttribute("aria-label", theme === "dark" ? "Switch to light mode" : "Switch to dark mode");
    }

    function applyLanguage(language) {
        if (!translations[language]) language = "en";
        root.dataset.language = language;
        document.documentElement.lang = language;
        localStorage.setItem("avena-language", language);
        if (languageSelect) languageSelect.value = language;
        document.querySelectorAll("[data-i18n]").forEach(el => {
            const value = translations[language][el.dataset.i18n];
            if (value) el.textContent = value;
        });
    }

    applyTheme(localStorage.getItem("avena-theme") || "light");
    applyLanguage(localStorage.getItem("avena-language") || "en");

    themeToggle?.addEventListener("click", () => applyTheme(root.dataset.theme === "dark" ? "light" : "dark"));
    languageSelect?.addEventListener("change", e => applyLanguage(e.target.value));

    navToggle?.addEventListener("click", () => {
        const open = navLinks.classList.toggle("open");
        navToggle.setAttribute("aria-expanded", open ? "true" : "false");
    });
    document.addEventListener("click", event => {
        if (navToggle && navLinks && !navToggle.contains(event.target) && !navLinks.contains(event.target)) {
            navLinks.classList.remove("open");
            navToggle.setAttribute("aria-expanded", "false");
        }
    });

    const showRegister = document.getElementById("showRegister");
    const registerForm = document.getElementById("registerForm");
    showRegister?.addEventListener("click", () => {
        const hidden = registerForm.hasAttribute("hidden");
        if (hidden) registerForm.removeAttribute("hidden"); else registerForm.setAttribute("hidden", "");
        showRegister.scrollIntoView({ behavior: "smooth", block: "nearest" });
    });
})();
