// Function to reset the chatbot to the initial state
function resetChatbotState() {
    clearChatBody();
    displayMessageBubble('Welcome to GEDSI HUB! How may I help you today?'); // Display initial "Hello" message

}

// Function to clear the chatbot body
function clearChatBody() {
    const chatbotBody = document.querySelector('.chatbot-body');
    chatbotBody.innerHTML = ''; // Clear all content from the chatbot body
}

// Function to display a message in a chat bubble
function displayMessageBubble(message) {
    const chatbotBody = document.querySelector('.chatbot-body');

    const chatMessage = document.createElement('div');
    chatMessage.classList.add('chat-message', 'd-flex', 'mt-3', 'fade-in');

    const botAvatar = document.createElement('img');
    botAvatar.src = 'https://picsum.photos/50/50';
    botAvatar.classList.add('bot-avatar');

    const messageBubble = document.createElement('div');
    messageBubble.classList.add('message-bubble');

    const messageText = document.createElement('p');
    messageText.classList.add('message-text');
    messageText.textContent = message;

    messageBubble.appendChild(messageText);
    chatMessage.appendChild(botAvatar);
    chatMessage.appendChild(messageBubble);

    chatbotBody.appendChild(chatMessage);
    chatbotBody.scrollTop = chatbotBody.scrollHeight; // Scroll to the bottom

    // Delay for animation
    setTimeout(() => {
        chatMessage.classList.add('show'); // Trigger fade-in effect
    }, 50); // Adjust delay as needed
}

// Function to display user message labels
function displayUserMessageLabel(label) {
    const chatbotBody = document.querySelector('.chatbot-body');

    const userMessage = document.createElement('div');
    userMessage.classList.add('user-chat-message', 'd-flex', 'mt-3', 'fade-in');

    const messageBubble = document.createElement('div');
    messageBubble.classList.add('user-message-bubble');

    const messageText = document.createElement('p');
    messageText.classList.add('message-text');
    messageText.textContent = label;

    messageBubble.appendChild(messageText);
    userMessage.appendChild(messageBubble);
    chatbotBody.appendChild(userMessage);
    chatbotBody.scrollTop = chatbotBody.scrollHeight; // Scroll to the bottom

    // Delay for animation
    setTimeout(() => {
        userMessage.classList.add('show'); // Trigger fade-in effect
    }, 50); // Adjust delay as needed
}


// Event listener for FAQ button
document.getElementById('faqButton').addEventListener('click', function () {
    displayUserMessageLabel("Frequently Asked Questions");
    loadFAQs();
});

// Event listener for Modules button
document.getElementById('modulesButton').addEventListener('click', function () {
    displayUserMessageLabel("Modules");
    loadModules();
});

// Event listener for Contact button
document.getElementById('contactButton').addEventListener('click', function () {
    displayUserMessageLabel("Contact Information");
    loadContactInfo();
});

// Close button logic
document.getElementById('chatbotExit').addEventListener('click', function () {
    const chatbotInterface = document.getElementById('chatbotInterface');
    chatbotInterface.classList.remove('show');
    setTimeout(() => (chatbotInterface.style.display = 'none'), 300);
    resetChatbotState(); // Reset to initial state
});

// Reset button logic
document.getElementById('resetButton').addEventListener('click', function () {
    resetChatbotState(); // Reset the chatbot
});

// Function to load FAQs
async function loadFAQs() {
    const chatbotBody = document.querySelector('.chatbot-body');
    const faqList = document.createElement('div');
    faqList.classList.add('option-chat-message', 'd-flex', 'mt-3', 'fade-in');

    try {
        const response = await fetch('/api/chatbot/faqs', { method: 'GET', headers: { 'Content-Type': 'application/json' } });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();
        data.faqs.forEach((faq) => {


            const button = document.createElement('button');
            button.classList.add('option-message-bubble');
            button.textContent = faq.question;
            button.addEventListener('click', function () {
                displayMessageBubble(faq.answer);
            });

            
            faqList.appendChild(button);
        });
    } catch (error) {
        console.error('Error fetching FAQs:', error);
        faqList.innerText = 'Error fetching FAQs';
    }

    chatbotBody.appendChild(faqList);
    chatbotBody.scrollTop = chatbotBody.scrollHeight; // Scroll to the bottom

    // Delay for animation
    setTimeout(() => {
        faqList.classList.add('show'); // Trigger fade-in effect
    }, 50); // Adjust delay as needed
}


// Event listener for the chatbot toggle button
document.getElementById('chatbotToggle').addEventListener('click', function () {
    const chatbotInterface = document.getElementById('chatbotInterface');
    const chatbotButton = document.querySelector('.chatbot-button');

    chatbotButton.classList.add('shrink'); // Add shrink animation
    setTimeout(() => chatbotButton.classList.remove('shrink'), 200);

    if (!chatbotInterface.classList.contains('show')) {
        chatbotInterface.style.display = 'block';
        setTimeout(() => chatbotInterface.classList.add('show'), 50);
    } else {
        chatbotInterface.classList.remove('show');

    }
});


// Function to load modules
async function loadModules() {
    const chatbotBody = document.querySelector('.chatbot-body');
    const modulesList = document.createElement('div');
    modulesList.classList.add('option-chat-message', 'd-flex', 'mt-3', 'fade-in');

    try {
        const response = await fetch('/api/chatbot/modules', { method: 'GET', headers: { 'Content-Type': 'application/json' } });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();
        data.modules.forEach((module) => {
            const button = document.createElement('a');
            button.classList.add('option-message-bubble');
            button.textContent = `${module.title}`;
            modulesList.appendChild(button);
        });
    } catch (error) {
        console.error('Error fetching Modules:', error);
        modulesList.innerText = 'Error fetching Modules';
    }

    chatbotBody.appendChild(modulesList);
    chatbotBody.scrollTop = chatbotBody.scrollHeight; // Scroll to the bottom

    // Delay for animation
    setTimeout(() => {
        modulesList.classList.add('show'); // Trigger fade-in effect
    }, 50); // Adjust delay as needed
}

// Function to load contact information
async function loadContactInfo() {
    const chatbotBody = document.querySelector('.chatbot-body');
    const contactInfo = document.createElement('div');
    contactInfo.classList.add('option-chat-message', 'd-flex', 'mt-3', 'fade-in');

    try {
        const response = await fetch('/api/chatbot/contact', { method: 'GET', headers: { 'Content-Type': 'application/json' } });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();
        const contact = data.contactInfo;
        contactInfo.innerHTML = `
            <div class="contact-message-bubble">
                <p class="contact-message-text"><strong>Email:</strong> ${contact.supportEmail ?? 'N/A'}</p>
                <p class="contact-message-text"><strong>Phone:</strong> ${contact.phoneNumber ?? 'N/A'}</p>
                <p class="contact-message-text">
                    <strong>Facebook:</strong> <a href="${contact.facebook}" target="_blank">@gadpup</a>
                </p>
                <p class="contact-message-text">
                    <strong>TikTok:</strong> <a href="${contact.tikTok}" target="_blank">@pup.gado</a>
                </p>
                <p class="contact-message-text">
                    <strong>Instagram:</strong> <a href="${contact.instagram}" target="_blank">@pupgadofficial</a>
                </p>
                <p class="contact-message-text">
                    <strong>X (Twitter):</strong> <a href="${contact.x}" target="_blank">@PUPGADO</a>
                </p>
                <p class="contact-message-text">
                    <strong>Website:</strong> <a href="${contact.website}" target="_blank">pup.gado.com.ph</a>
                </p>
            </div>
            `;
    } catch (error) {
        console.error('Error fetching contact info:', error);
        contactInfo.innerText = 'Error fetching contact info';

    }

    chatbotBody.appendChild(contactInfo);
    chatbotBody.scrollTop = chatbotBody.scrollHeight; // Scroll to the bottom

    // Delay for animation
    setTimeout(() => {
        contactInfo.classList.add('show'); // Trigger fade-in effect
    }, 50); // Adjust delay as needed
}

// Initialize event listeners on page load
document.addEventListener('DOMContentLoaded', resetChatbotState);