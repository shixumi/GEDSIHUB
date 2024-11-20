document.querySelectorAll(".like-btn").forEach(button => {
    button.addEventListener("click", async () => {
        const postId = button.dataset.postId;
        const hasLiked = button.dataset.hasLiked === "true"; // Convert string to boolean
        const url = `/ForumPost/ToggleLike/${postId}`;

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                console.error("Failed to toggle like status");
                return;
            }

            const data = await response.json();
            if (data.success) {
                // Update the button's dataset and icon
                button.dataset.hasLiked = (!hasLiked).toString(); // Toggle the hasLiked value
                const icon = button.querySelector("i");
                if (data.hasLiked) {
                    icon.classList.remove("la-heart-o");
                    icon.classList.add("la-heart");
                } else {
                    icon.classList.remove("la-heart");
                    icon.classList.add("la-heart-o");
                }

                // Update the like count
                const likeCount = document.querySelector(`#likeCount-${postId}`);
                if (likeCount) {
                    likeCount.textContent = data.newLikeCount;
                }
            }
        } catch (error) {
            console.error("Error toggling like status:", error);
        }
    });
});
