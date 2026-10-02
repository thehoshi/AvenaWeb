(() => {
    const root = document.documentElement;
    const themeToggle = document.getElementById("themeToggle");
    const languageSelect = document.getElementById("languageSelect");
    const navToggle = document.getElementById("navToggle");
    const navLinks = document.getElementById("navLinks");

    const translations = {
        en: {
            "home.est_2026_an_open_forum": "Est. 2026 — An open forum",
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
            "home.created_text": "A small collective of writers, editors, and researchers who believe public debate deserves a calmer home.",
            "home.mission_text": "To turn scattered reactions into informed conversation — one article at a time.",
            "home.purpose_text": "A space where evidence and opinion sit side by side, so readers can weigh both before they speak.",
            "home.year_text": "Founded in 2026, built for the conversations still ahead of us.",
            "home.quote": "\"Every opinion matters when it is supported by facts.\"",
            "nav.home": "Home", "nav.news": "News", "nav.profile": "Profile", "nav.login": "Login", "nav.logout": "Logout",
            "footer.copy": "Avena © 2026", "footer.privacy": "Privacy Policy", "footer.terms": "Terms",
            "common.or": "or", "common.read_more": "Read More", "common.contact": "Contact", "common.last_updated": "Last updated: September 16, 2026",
            "form.username": "Username", "form.password": "Password", "form.name": "Name",
            "login.welcome": "Welcome back", "login.subtitle": "Log in to continue to Avena", "login.button": "Login",
            "register.create": "Create an account", "register.title": "Create your Avena account", "register.subtitle": "Join the conversation.", "register.button": "Create account", "register.have_account": "Already have an account?",
            "profile.display_name": "Display name", "profile.avatar_image": "Avatar image", "profile.save": "Save changes",
            "article.genre": "Genre", "article.author": "Author", "article.anonymous": "Anonymous", "article.date": "Published", "article.views": "Views", "article.comments": "Comments", "article.no_comments": "No comments yet — be the first to share your take.", "article.leave_comment": "Leave a comment", "article.post_comment": "Post comment", "article.login_to_comment": "Login", "article.to_comment": "to leave a comment.", "article.comment_placeholder": "Write your comment... (Enter to post, Shift+Enter for a new line)",
            "genre.empty": "No articles in this genre yet.",
            "page.home": "Avena", "page.profile": "Profile", "page.register": "Register", "page.privacy": "Privacy Policy", "page.terms": "Terms",
            "privacy.title": "Privacy Policy", "privacy.collect": "Information We Collect", "privacy.use": "How We Use Your Information", "privacy.retention": "Data Retention", "privacy.third_party": "Third-Party Services", "privacy.rights": "Your Rights",
            "privacy.intro": "Avena (\"we\", \"us\") operates this website. This policy explains what information we collect and how we use it.",
            "privacy.account_data": "Account data: username, display name, and password (stored hashed) when you register.", "privacy.content_data": "Content you submit: news posts, comments, and images you upload.", "privacy.usage_data": "Usage data: pages viewed, article view counts.", "privacy.cookies": "Cookies: we use a session cookie to keep you logged in.", "privacy.use_account": "To provide and maintain your account and let you post content.", "privacy.use_display": "To display your username alongside comments and posts you make.", "privacy.use_improve": "To improve the site's functionality.", "privacy.no_sell": "We do not sell your personal data to third parties.", "privacy.retention_text": "We retain your account and content data for as long as your account is active. You may request deletion by contacting us at", "privacy.third_party_text": "Our site may link to third-party platforms (Instagram, TikTok). We are not responsible for their privacy practices.", "privacy.rights_text": "You may request access to, correction of, or deletion of your personal data at any time by contacting", "privacy.contact_text": "Questions about this policy? Contact us at",
            "terms.title": "Terms of Use", "terms.intro": "By accessing Avena, you agree to these terms.", "terms.accounts_title": "1. Accounts", "terms.accounts_text": "You must provide accurate information when registering. You are responsible for maintaining the security of your account.", "terms.content_title": "2. User Content", "terms.content_text": "You retain ownership of content you post (articles, comments, images), but grant Avena a license to display it on the platform. You are solely responsible for what you post.", "terms.prohibited_title": "3. Prohibited Conduct", "terms.prohibited_text": "You may not post content that is illegal, harassing, hateful, or infringes on others' rights. We reserve the right to remove content and suspend accounts that violate these terms.", "terms.moderation_title": "4. Content Moderation", "terms.moderation_text": "We may review, edit, or remove any content at our discretion, though we do not guarantee active moderation of all posts.", "terms.disclaimer_title": "5. Disclaimer", "terms.disclaimer_text": "Content on Avena reflects the opinions of individual authors and does not represent the views of Avena as a platform. We are not liable for inaccuracies in user-submitted content.", "terms.changes_title": "6. Changes to These Terms", "terms.changes_text": "We may update these terms from time to time. Continued use of the site constitutes acceptance of the updated terms.", "terms.contact_title": "7. Contact", "terms.contact_text": "Questions? Contact us at"
        },
        ru: {
            "home.est_2026_an_open_forum": "Основано в 2026 — открытый форум", "home.opinions_shape_the_future": "Мнения формируют будущее.", "home.stay_informed_stay_engaged": "Будьте в курсе. Участвуйте.", "home.read_latest_news": "Читать последние новости", "home.about_the_project": "О проекте", "home.why_this_platform_exists": "Зачем существует эта платформа", "home.who_created_it": "Кто её создал", "home.mission": "Миссия", "home.purpose": "Назначение", "home.year": "Год", "home.latest_news": "Последние новости", "home.what_people_are_talking_about": "О чём говорят", "home.no_articles_have_been_published_yet_check_back_soon": "Статей пока нет. Загляните позже.", "home.browse_by_topic": "По темам", "home.no_topics_yet": "Пока нет тем.", "home.contact": "Контакты", "home.let_s_talk": "Давайте поговорим", "home.created_text": "Небольшая команда авторов, редакторов и исследователей, которые считают, что публичной дискуссии нужен более спокойный дом.", "home.mission_text": "Превращать разрозненные реакции в содержательный разговор — по одной статье за раз.", "home.purpose_text": "Пространство, где факты и мнения существуют рядом, чтобы читатели могли взвесить и то и другое.", "home.year_text": "Основано в 2026 году для разговоров, которые ещё впереди.",
            "home.quote": "\"Каждое мнение имеет значение, когда оно подкреплено фактами.\"",
            "nav.home": "Главная", "nav.news": "Новости", "nav.profile": "Профиль", "nav.login": "Войти", "nav.logout": "Выйти", "footer.copy": "Avena © 2026", "footer.privacy": "Политика конфиденциальности", "footer.terms": "Условия", "common.or": "или", "common.read_more": "Читать далее", "common.contact": "Контакты", "common.last_updated": "Последнее обновление: 16 сентября 2026", "form.username": "Имя пользователя", "form.password": "Пароль", "form.name": "Имя", "login.welcome": "С возвращением", "login.subtitle": "Войдите, чтобы продолжить в Avena", "login.button": "Войти", "register.create": "Создать аккаунт", "register.title": "Создайте аккаунт Avena", "register.subtitle": "Присоединяйтесь к разговору.", "register.button": "Создать аккаунт", "register.have_account": "Уже есть аккаунт?", "profile.display_name": "Отображаемое имя", "profile.avatar_image": "Изображение профиля", "profile.save": "Сохранить изменения", "article.genre": "Жанр", "article.author": "Автор", "article.anonymous": "Аноним", "article.date": "Опубликовано", "article.views": "Просмотры", "article.comments": "Комментарии", "article.no_comments": "Комментариев пока нет — станьте первым, кто поделится мнением.", "article.leave_comment": "Оставить комментарий", "article.post_comment": "Опубликовать комментарий", "article.login_to_comment": "Войти", "article.to_comment": ", чтобы оставить комментарий.", "article.comment_placeholder": "Напишите комментарий... (Enter — отправить, Shift+Enter — новая строка)", "genre.empty": "В этом жанре пока нет статей.", "page.home": "Avena", "page.profile": "Профиль", "page.register": "Регистрация", "page.privacy": "Политика конфиденциальности", "page.terms": "Условия",
            "privacy.title": "Политика конфиденциальности", "privacy.collect": "Какие данные мы собираем", "privacy.use": "Как мы используем ваши данные", "privacy.retention": "Хранение данных", "privacy.third_party": "Сторонние сервисы", "privacy.rights": "Ваши права", "privacy.intro": "Avena (\"мы\") управляет этим сайтом. Здесь описано, какие данные мы собираем и как используем их.", "privacy.account_data": "Данные аккаунта: имя пользователя, отображаемое имя и пароль (в хешированном виде), указанные при регистрации.", "privacy.content_data": "Отправленный вами контент: статьи, комментарии и загруженные изображения.", "privacy.usage_data": "Данные использования: просмотренные страницы и количество просмотров статей.", "privacy.cookies": "Cookies: мы используем cookie сессии, чтобы сохранять вход в аккаунт.", "privacy.use_account": "Для работы аккаунта и предоставления возможности публиковать контент.", "privacy.use_display": "Чтобы показывать ваше имя пользователя рядом с комментариями и публикациями.", "privacy.use_improve": "Для улучшения работы сайта.", "privacy.no_sell": "Мы не продаём ваши персональные данные третьим лицам.", "privacy.retention_text": "Мы храним данные аккаунта и контент, пока аккаунт активен. Запросить удаление можно по адресу", "privacy.third_party_text": "На сайте могут быть ссылки на Instagram и TikTok. Мы не отвечаем за их правила конфиденциальности.", "privacy.rights_text": "Вы можете запросить доступ, исправление или удаление персональных данных, написав на", "privacy.contact_text": "Вопросы по политике? Пишите на",
            "terms.title": "Условия использования", "terms.intro": "Используя Avena, вы соглашаетесь с этими условиями.", "terms.accounts_title": "1. Аккаунты", "terms.accounts_text": "При регистрации необходимо указывать достоверную информацию. Вы отвечаете за безопасность своего аккаунта.", "terms.content_title": "2. Пользовательский контент", "terms.content_text": "Вы сохраняете права на опубликованный контент, но предоставляете Avena лицензию на его отображение. Вы несёте ответственность за свои публикации.", "terms.prohibited_title": "3. Запрещённые действия", "terms.prohibited_text": "Нельзя публиковать незаконный, преследующий, ненавистнический контент или материалы, нарушающие права других лиц. Мы можем удалить такой контент и приостановить аккаунты.", "terms.moderation_title": "4. Модерация контента", "terms.moderation_text": "Мы можем проверять, редактировать или удалять контент по своему усмотрению, но не гарантируем постоянную модерацию всех публикаций.", "terms.disclaimer_title": "5. Отказ от ответственности", "terms.disclaimer_text": "Контент Avena отражает мнения отдельных авторов и не является позицией платформы. Мы не отвечаем за неточности в пользовательском контенте.", "terms.changes_title": "6. Изменения условий", "terms.changes_text": "Мы можем время от времени обновлять эти условия. Продолжение использования сайта означает принятие обновлённых условий.", "terms.contact_title": "7. Контакты", "terms.contact_text": "Вопросы? Пишите на"
        },
        az: {
            "home.est_2026_an_open_forum": "2026-dan — Açıq forum", "home.opinions_shape_the_future": "Fikirlər gələcəyi formalaşdırır.", "home.stay_informed_stay_engaged": "Məlumatlı olun. Fəal olun.", "home.read_latest_news": "Son xəbərləri oxu", "home.about_the_project": "Layihə haqqında", "home.why_this_platform_exists": "Bu platforma niyə mövcuddur", "home.who_created_it": "Kim yaradıb", "home.mission": "Missiya", "home.purpose": "Məqsəd", "home.year": "İl", "home.latest_news": "Son xəbərlər", "home.what_people_are_talking_about": "Nədən danışırlar", "home.no_articles_have_been_published_yet_check_back_soon": "Hələ məqalə dərc edilməyib. Sonra yenidən baxın.", "home.browse_by_topic": "Mövzuya görə bax", "home.no_topics_yet": "Hələ mövzu yoxdur.", "home.contact": "Əlaqə", "home.let_s_talk": "Gəlin danışaq", "home.created_text": "İctimai müzakirənin daha sakit bir məkana layiq olduğuna inanan kiçik müəllif, redaktor və tədqiqatçı komandası.", "home.mission_text": "Səpələnmiş reaksiyaları hər dəfə bir məqalə olmaqla məlumatlı söhbətə çevirmək.", "home.purpose_text": "Fakt və fikirlərin yanaşı olduğu, oxucuların danışmazdan əvvəl hər ikisini ölçə bildiyi məkan.", "home.year_text": "2026-cı ildə gələcək söhbətlər üçün yaradılıb.",
            "home.quote": "\"Hər fikir faktlarla dəstəkləndikdə dəyərlidir.\"",
            "nav.home": "Ana səhifə", "nav.news": "Xəbərlər", "nav.profile": "Profil", "nav.login": "Daxil ol", "nav.logout": "Çıxış", "footer.copy": "Avena © 2026", "footer.privacy": "Məxfilik siyasəti", "footer.terms": "Şərtlər", "common.or": "və ya", "common.read_more": "Daha çox oxu", "common.contact": "Əlaqə", "common.last_updated": "Son yenilənmə: 16 sentyabr 2026", "form.username": "İstifadəçi adı", "form.password": "Şifrə", "form.name": "Ad", "login.welcome": "Xoş gəlmisiniz", "login.subtitle": "Avenaya davam etmək üçün daxil olun", "login.button": "Daxil ol", "register.create": "Hesab yarat", "register.title": "Avena hesabınızı yaradın", "register.subtitle": "Söhbətə qoşulun.", "register.button": "Hesab yarat", "register.have_account": "Artıq hesabınız var?", "profile.display_name": "Görünən ad", "profile.avatar_image": "Profil şəkli", "profile.save": "Dəyişiklikləri saxla", "article.genre": "Janr", "article.author": "Müəllif", "article.anonymous": "Anonim", "article.date": "Dərc edilib", "article.views": "Baxışlar", "article.comments": "Şərhlər", "article.no_comments": "Hələ şərh yoxdur — fikrinizi ilk paylaşan siz olun.", "article.leave_comment": "Şərh yaz", "article.post_comment": "Şərhi göndər", "article.login_to_comment": "Daxil ol", "article.to_comment": "şərh yazmaq üçün.", "article.comment_placeholder": "Şərhinizi yazın... (Enter — göndər, Shift+Enter — yeni sətir)", "genre.empty": "Bu janrda hələ məqalə yoxdur.", "page.home": "Avena", "page.profile": "Profil", "page.register": "Qeydiyyat", "page.privacy": "Məxfilik siyasəti", "page.terms": "Şərtlər",
            "privacy.title": "Məxfilik siyasəti", "privacy.collect": "Topladığımız məlumatlar", "privacy.use": "Məlumatlardan necə istifadə edirik", "privacy.retention": "Məlumatların saxlanması", "privacy.third_party": "Üçüncü tərəf xidmətləri", "privacy.rights": "Hüquqlarınız", "privacy.intro": "Avena (\"biz\") bu saytı idarə edir. Bu siyasət hansı məlumatları topladığımızı və onlardan necə istifadə etdiyimizi izah edir.", "privacy.account_data": "Hesab məlumatları: qeydiyyat zamanı istifadəçi adı, görünən ad və şifrənin hash edilmiş forması.", "privacy.content_data": "Göndərdiyiniz məzmun: məqalələr, şərhlər və yüklədiyiniz şəkillər.", "privacy.usage_data": "İstifadə məlumatları: baxılan səhifələr və məqalə baxışlarının sayı.", "privacy.cookies": "Cookies: hesabda girişinizi saxlamaq üçün sessiya cookie-dən istifadə edirik.", "privacy.use_account": "Hesabınızı təmin etmək və məzmun dərc etməyə imkan vermək üçün.", "privacy.use_display": "Şərhlər və paylaşımlarınızın yanında istifadəçi adınızı göstərmək üçün.", "privacy.use_improve": "Saytın funksionallığını yaxşılaşdırmaq üçün.", "privacy.no_sell": "Şəxsi məlumatlarınızı üçüncü tərəflərə satmırıq.", "privacy.retention_text": "Hesab və məzmun məlumatlarınızı hesab aktiv olduğu müddətdə saxlayırıq. Silinmə tələbi üçün", "privacy.third_party_text": "Saytda Instagram və TikTok kimi üçüncü tərəf platformalarına keçidlər ola bilər. Onların məxfilik qaydalarına görə məsuliyyət daşımırıq.", "privacy.rights_text": "Şəxsi məlumatlarınıza çıxış, düzəliş və ya silinmə tələbi üçün", "privacy.contact_text": "Siyasətlə bağlı suallar?",
            "terms.title": "İstifadə şərtləri", "terms.intro": "Avena-ya daxil olmaqla bu şərtlərlə razılaşırsınız.", "terms.accounts_title": "1. Hesablar", "terms.accounts_text": "Qeydiyyat zamanı düzgün məlumat verməlisiniz. Hesabınızın təhlükəsizliyinə görə məsuliyyət daşıyırsınız.", "terms.content_title": "2. İstifadəçi məzmunu", "terms.content_text": "Dərc etdiyiniz məzmuna sahiblik sizdə qalır, lakin Avena-ya onu platformada göstərmək üçün lisenziya verirsiniz. Paylaşdığınız məzmuna görə yalnız siz məsuliyyət daşıyırsınız.", "terms.prohibited_title": "3. Qadağan olunan davranış", "terms.prohibited_text": "Qanunsuz, təqibedici, nifrət məzmunlu və ya başqalarının hüquqlarını pozan materiallar dərc etmək olmaz. Bu şərtləri pozan məzmunu silə və hesabları dayandıra bilərik.", "terms.moderation_title": "4. Məzmun moderasiyası", "terms.moderation_text": "İstənilən məzmunu öz mülahizəmizə görə yoxlaya, redaktə edə və ya silə bilərik, lakin bütün paylaşımların aktiv moderasiyasına zəmanət vermirik.", "terms.disclaimer_title": "5. Məsuliyyət məhdudiyyəti", "terms.disclaimer_text": "Avena məzmunu ayrı-ayrı müəlliflərin fikirlərini əks etdirir və platformanın mövqeyini ifadə etmir. İstifadəçi məzmunundakı qeyri-dəqiqliklərə görə məsuliyyət daşımırıq.", "terms.changes_title": "6. Şərtlərdə dəyişikliklər", "terms.changes_text": "Bu şərtləri zaman-zaman yeniləyə bilərik. Saytdan istifadəni davam etdirmək yenilənmiş şərtləri qəbul etdiyiniz deməkdir.", "terms.contact_title": "7. Əlaqə", "terms.contact_text": "Suallar?"
        }
    };

    const genreTranslations = {
        en: { Technology: "Technology", Education: "Education", "Language & Culture": "Language & Culture", "Law & AI": "Law & AI", "International Relations": "International Relations", "Science & Health": "Science & Health" },
        ru: { Technology: "Технологии", Education: "Образование", "Language & Culture": "Язык и культура", "Law & AI": "Право и ИИ", "International Relations": "Международные отношения", "Science & Health": "Наука и здоровье" },
        az: { Technology: "Texnologiya", Education: "Təhsil", "Language & Culture": "Dil və mədəniyyət", "Law & AI": "Hüquq və süni intellekt", "International Relations": "Beynəlxalq münasibətlər", "Science & Health": "Elm və sağlamlıq" }
    };

    const viewLabels = {
        en: n => `${n} ${n === 1 ? "view" : "views"}`,
        ru: n => `${n} ${n % 10 === 1 && n % 100 !== 11 ? "просмотр" : (n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? "просмотра" : "просмотров")}`,
        az: n => `${n} baxış`
    };

    function applyTheme(theme) {
        theme = theme === "dark" ? "dark" : "light";
        root.dataset.theme = theme;
        localStorage.setItem("avena-theme", theme);
        if (themeToggle) themeToggle.textContent = theme === "dark" ? "☀" : "☾";
    }

    const serverErrors = {
        "Username and password are required.": { en: "Username and password are required.", ru: "Введите имя пользователя и пароль.", az: "İstifadəçi adı və şifrə tələb olunur." },
        "Invalid username or password.": { en: "Invalid username or password.", ru: "Неверное имя пользователя или пароль.", az: "İstifadəçi adı və ya şifrə yanlışdır." },
        "Unable to connect to the database. Please try again later.": { en: "Unable to connect to the database. Please try again later.", ru: "Не удалось подключиться к базе данных. Попробуйте позже.", az: "Verilənlər bazasına qoşulmaq mümkün olmadı. Sonra yenidən cəhd edin." },
        "All registration fields are required.": { en: "All registration fields are required.", ru: "Заполните все поля регистрации.", az: "Qeydiyyat üçün bütün sahələri doldurun." },
        "Username must contain at least 3 characters.": { en: "Username must contain at least 3 characters.", ru: "Имя пользователя должно содержать не менее 3 символов.", az: "İstifadəçi adı ən azı 3 simvoldan ibarət olmalıdır." },
        "Password must contain at least 6 characters.": { en: "Password must contain at least 6 characters.", ru: "Пароль должен содержать не менее 6 символов.", az: "Şifrə ən azı 6 simvoldan ibarət olmalıdır." },
        "Username already exists.": { en: "Username already exists.", ru: "Такое имя пользователя уже существует.", az: "Bu istifadəçi adı artıq mövcuddur." },
        "Unable to create the account. Please try again later.": { en: "Unable to create the account. Please try again later.", ru: "Не удалось создать аккаунт. Попробуйте позже.", az: "Hesab yaratmaq mümkün olmadı. Sonra yenidən cəhd edin." }
    };

    function translateStatic(language) {
        const dict = translations[language] || translations.en;
        document.querySelectorAll("[data-i18n]").forEach(el => {
            const value = dict[el.dataset.i18n];
            if (value) el.textContent = value;
        });
        document.querySelectorAll("[data-i18n-placeholder]").forEach(el => {
            const value = dict[el.dataset.i18nPlaceholder];
            if (value) el.setAttribute("placeholder", value);
        });
        document.querySelectorAll("[data-server-message]").forEach(el => {
            const message = el.dataset.serverMessage;
            const translated = serverErrors[message]?.[language];
            if (translated) el.textContent = translated;
        });
        document.querySelectorAll("[data-genre]").forEach(el => {
            const raw = el.dataset.genre;
            el.textContent = genreTranslations[language]?.[raw] || raw;
        });
        document.querySelectorAll(".localized-views").forEach(el => {
            const count = Number(el.dataset.count || 0);
            el.textContent = viewLabels[language](count);
        });
        document.querySelectorAll(".localized-date").forEach(el => {
            const date = new Date(el.dataset.date);
            if (Number.isNaN(date.getTime())) return;
            const locales = { en: "en-US", ru: "ru-RU", az: "az-AZ" };
            el.textContent = new Intl.DateTimeFormat(locales[language], { year: "numeric", month: "long", day: "numeric" }).format(date);
        });
        const titleKey = document.body.dataset.pageTitle;
        if (titleKey && dict[titleKey]) document.title = `${dict[titleKey]} - Avena`;
    }

    function applyLanguage(language) {
        if (!translations[language]) language = "en";
        root.dataset.language = language;
        document.documentElement.lang = language;
        localStorage.setItem("avena-language", language);
        if (languageSelect) languageSelect.value = language;
        translateStatic(language);
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
})();
