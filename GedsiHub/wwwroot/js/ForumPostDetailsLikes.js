document.addEventListener("DOMContentLoaded", () => {
    const likeButton = document.querySelector(".like-btn");
    if (likeButton) {
        likeButton.addEventListener("click", async () => {
            const postId = likeButton.dataset.postId;
            const hasLiked = likeButton.dataset.hasLiked === "true"; // Convert string to boolean
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
                    // Toggle the `hasLiked` value
                    likeButton.dataset.hasLiked = (!hasLiked).toString();

                    // Update the icon
                    const icon = likeButton.querySelector("i");
                    if (data.hasLiked) {
                        icon.classList.remove("la-heart-o");
                        icon.classList.add("la-heart");
                    } else {
                        icon.classList.remove("la-heart");
                        icon.classList.add("la-heart-o");
                    }

                    // Update the like count
                    const likeCount = document.querySelector("#likeCount");
                    if (likeCount) {
                        likeCount.textContent = data.newLikeCount;
                    }
                }
            } catch (error) {
                console.error("Error toggling like status:", error);
            }
        });
    }
});
