import { check_token_validity, get_token  } from "./helper_modules.js";


class MainWindow {
    constructor() {
        this.initElements();
        this.bindEvents();
        this.onDomLoaded(); // Викликаємо безпосередньо
    }

    initElements() {
        // Основні елементи
        this.contentContainer = document.getElementById('content');
        this.navMenu = document.getElementById('navMenu');
        this.loadingState = document.getElementById('loadingState');
        this.modalOverlay = document.getElementById('modalOverlay');
        this.modalBody = document.getElementById('modalBody');
        this.modalTitle = document.getElementById('modalTitle');
        this.modalClose = document.getElementById('modalClose');
        
        // Кнопки
        this.menuToggle = document.getElementById('menuToggle');
        this.logoutBtn = document.getElementById('logoutBtn');
        this.quickAddBtn = document.getElementById('quickAddBtn');
        this.notificationBtn = document.getElementById('notificationBtn');
        
        // Інформаційні поля
        this.userName = document.getElementById('userName');
        this.email = document.getElementById('email');
        this.userAvatar = document.getElementById('userAvatar');
        this.serverStatus = document.getElementById('serverStatus');
        this.lastUpdate = document.getElementById('lastUpdate');
        this.notificationCount = document.getElementById('notificationCount');
    }

    bindEvents() {
        const navItems = this.navMenu.querySelectorAll('.nav-item');
        navItems.forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                const link = item.querySelector('.nav-link');
                const section = link.getAttribute('href').substring(1); // видаляємо #
                this.showSection(section);
                
                // Активуємо обраний елемент
                navItems.forEach(nav => nav.classList.remove('active'));
                item.classList.add('active');
            });
        });

        // Додаємо обробники подій для інших елементів
        this.menuToggle?.addEventListener('click', () => this.toggleMenu());
        this.logoutBtn?.addEventListener('click', () => this.logout());
        this.quickAddBtn?.addEventListener('click', () => this.quickAdd());
        this.notificationBtn?.addEventListener('click', () => this.showNotifications());
        this.modalClose?.addEventListener('click', () => this.closeModal());
    }
    
    onDomLoaded() {
        alert("Успішний вхід в аккаунт");
        this.initializeApp();
    }
    
    initializeApp() {
        console.log("Initializing application...");

        this.loadUserData();

    }
    
    async loadUserData() {
        this.userName.innerText = localStorage.getItem("full_name")
        this.email.innerText = localStorage.getItem("email")
        if (this.loadingState) {
            this.loadingState.style.display = 'none';
        }
    }

    
    toggleMenu() {
        // Логіка перемикання меню
        this.navMenu?.classList.toggle('active');
    }
    
    logout() {
        // Логіка виходу
        console.log("Logging out...");
    }
    
    quickAdd() {
        // Логіка швидкого додавання
        console.log("Quick add...");
    }
    
    showNotifications() {
        // Логіка показу сповіщень
        console.log("Showing notifications...");
    }
    
    closeModal() {
        // Закриття модального вікна
        this.modalOverlay.style.display = 'none';
    }

    loadSettings() {
        let main = document.createElement("div");
        let name = document.createElement("p").innerText = localStorage.getItem("full_name")
        let email = document.createElement("p").innerText = localStorage.getItem("email")
        let username = document.createElement("p").innerText = localStorage.getItem("username")
        let phone = document.createElement("p").innerText = localStorage.getItem("phone")
        main.appendChild(name, email, username, phone)

        this.contentContainer.appendChild(main)
    }

    showSection(section) {
        // Приховуємо всі секції
        const sections = [
            this.profileContent,
            this.analyticsContent,
            this.managementContent,
            this.settingsContent
        ];
        
        sections.forEach(sec => {
            if (sec) sec.style.display = 'none';
        });
        
        // Показуємо обрану секцію
        let selectedSection = null;
        let title = 'Головна панель';
        
        switch(section) {
            case 'profile':
                selectedSection = this.profileContent;
                title = 'Профіль користувача';
                break;
            case 'analytics':
                selectedSection = this.analyticsContent;
                title = 'Аналітика бізнесу';
                break;
            case 'management':
                selectedSection = this.managementContent;
                title = 'Управління бізнесом';
                break;
            case 'settings':
                selectedSection = this.settingsContent;
                title = 'Налаштування системи';
                break;
            case 'finance':
                title = 'Фінанси';
                break;
            case 'clients':
                title = 'Клієнти';
                break;
            case 'projects':
                title = 'Проєкти';
                break;
            case 'reports':
                title = 'Звіти';
                break;
        }
        
        if (selectedSection) {
            selectedSection.style.display = 'block';
        }
        
        // Оновлюємо заголовок сторінки
        if (this.pageTitle) {
            this.pageTitle.textContent = title;
        }
    }
}

// Створюємо екземпляр класу
const myWindows = new MainWindow();


