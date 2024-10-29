// Log a message when the script is loaded
console.log("JavaScript Loaded: Starting to fetch data...");

// Function to reset the chatbot state
function resetChatbotState() {
    // Hide all sections (FAQ, Modules, Contact)
    document.getElementById('faqSection').style.display = 'none';
    document.getElementById('modulesSection').style.display = 'none';
    document.getElementById('contactSection').style.display = 'none';

    // Show the options section again
    document.getElementById('chatOptions').style.display = 'block';

    // Clear any input in the text field
    document.querySelector('.chat-text-input').value = '';

    // Clear any dynamically added content (like FAQs, Modules, or Contact info)
    document.getElementById('faqList').innerHTML = '';
    document.getElementById('modulesList').innerHTML = '';
    document.getElementById('contactInfo').innerHTML = 'Fetching contact info...';

    // Show the chat message again when chatbot is reset
    const chatMessage = document.querySelector('.chat-message');
    if (chatMessage) {
        chatMessage.style.display = 'block';
    }
}

// Function to hide chat message
function hideChatMessage() {
    const chatMessage = document.querySelector('.chat-message');
    if (chatMessage) {
        chatMessage.style.display = 'none';
    }
}

// Toggle chatbot interface
document.getElementById('chatbotToggle').addEventListener('click', function() {
    const chatbotInterface = document.getElementById('chatbotInterface');
    chatbotInterface.style.display = (chatbotInterface.style.display === 'block') ? 'none' : 'block';
});

// Close button logic: reset the chatbot state and hide the interface
document.getElementById('chatbotExit').addEventListener('click', function() {
    const chatbotInterface = document.getElementById('chatbotInterface');
    chatbotInterface.style.display = 'none';

    // Reset the chatbot state
    resetChatbotState();
});

// Function to fetch contact information
async function loadContactInfo() {
    try {
        console.log("Fetching contact info...");
        const response = await fetch('/api/chatbot/contact', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Contact Info Response Data:", data);

        // Access contactInfo object correctly
        const contact = data.contactInfo;
        document.getElementById('contactInfo').innerHTML = `
        <ul>
            <li class="list-group-item"><strong>Email:</strong> ${contact.supportEmail ?? 'N/A'}</li>
            <li class="list-group-item"><strong>Phone:</strong> ${contact.phoneNumber ?? 'N/A'}</li>
            <li class="list-group-item"><strong>Facebook:</strong> <a href="${contact.facebook}" target="_blank">@gadpup</a></li>
            <li class="list-group-item"><strong>TikTok:</strong> <a href="${contact.tikTok}" target="_blank">@pup.gado</a></li>
            <li class="list-group-item"><strong>Instagram:</strong> <a href="${contact.instagram}" target="_blank">@pupgadofficial</a></li>
            <li class="list-group-item"><strong>X (Twitter):</strong> <a href="${contact.x}" target="_blank">@PUPGADO</a></li>
            <li class="list-group-item"><strong>Website:</strong> <a href="${contact.website}" target="_blank">Website</a></li>
        </ul>
        `;
    } catch (error) {
        console.error('Error fetching contact info:', error);
        document.getElementById('contactInfo').innerText = 'Error fetching contact info';
    }
}

// Function to fetch FAQs
async function loadFAQs() {
    try {
        console.log("Fetching FAQs...");
        const response = await fetch('/api/chatbot/faqs', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("FAQs Response Data:", data);

        const faqList = document.getElementById('faqList');
        faqList.innerHTML = ''; // Clear previous data
        data.faqs.forEach(faq => {
            const li = document.createElement('li');
            li.classList.add('list-group-item');
            const answer = faq.answer ? faq.answer.replace(/'/g, "\\'") : "Answer not available";
            li.innerHTML = `
                <button class="btn btn-link" onclick="alert('Answer: ${answer}')">
                    ${faq.question}
                </button>
            `;
            faqList.appendChild(li);
        });
    } catch (error) {
        console.error('Error fetching FAQs:', error);
        document.getElementById('faqList').innerText = 'Error fetching FAQs';
    }
}

// Function to fetch Modules
async function loadModules() {
    try {
        console.log("Fetching Modules...");
        const response = await fetch('/api/chatbot/modules', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Modules Response Data:", data);

        const modulesList = document.getElementById('modulesList');
        modulesList.innerHTML = ''; // Clear previous data
        data.modules.forEach(module => {
            const li = document.createElement('li');
            li.classList.add('list-group-item');
            li.innerHTML = `<button class="btn btn-link">${module.title}</button>`;
            modulesList.appendChild(li);
        });
    } catch (error) {
        console.error('Error fetching Modules:', error);
        document.getElementById('modulesList').innerText = 'Error fetching Modules';
    }
}

// Initialize event listeners for chatbot options
function initializeEventListeners() {
    // Event Listeners for buttons
    document.getElementById('faqButton').addEventListener('click', function () {
        document.getElementById('faqSection').style.display = 'block';
        document.getElementById('modulesSection').style.display = 'none';
        document.getElementById('contactSection').style.display = 'none';
        loadFAQs();
        hideChatMessage();  // Hide chat message when FAQ button is clicked
    });

    document.getElementById('modulesButton').addEventListener('click', function () {
        document.getElementById('faqSection').style.display = 'none';
        document.getElementById('modulesSection').style.display = 'block';
        document.getElementById('contactSection').style.display = 'none';
        loadModules();
        hideChatMessage();  // Hide chat message when Modules button is clicked
    });

    document.getElementById('contactButton').addEventListener('click', function () {
        document.getElementById('faqSection').style.display = 'none';
        document.getElementById('modulesSection').style.display = 'none';
        document.getElementById('contactSection').style.display = 'block';
        loadContactInfo();
        hideChatMessage();  // Hide chat message when Contact button is clicked
    });

    // Initially hide sections
    document.getElementById('faqSection').style.display = 'none';
    document.getElementById('modulesSection').style.display = 'none';
    document.getElementById('contactSection').style.display = 'none';
}

// Ensure the DOM is fully loaded before running the script
document.addEventListener('DOMContentLoaded', initializeEventListeners);
