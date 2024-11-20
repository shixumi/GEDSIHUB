document.addEventListener("DOMContentLoaded", () => {
    // Like button click handler
    document.querySelectorAll(".like-button").forEach((button) => {
        button.addEventListener("click", (event) => {
            const postId = event.target.dataset.postId;

            fetch(`/ForumPost/LikePost/${postId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest",
                },
            })
                .then((response) => {
                    if (!response.ok) {
                        throw new Error("Failed to like the post.");
                    }
                    return response.json();
                })
                .then((data) => {
                    const likeCountElement = document.querySelector(
                        `#like-count-${postId}`
                    );
                    likeCountElement.textContent = data.newLikesCount;
                })
                .catch((error) => {
                    console.error("Error liking post:", error);
                });
        });
    });
});
