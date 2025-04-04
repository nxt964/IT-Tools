function toggleHeart(event, icon, toolId) {
    event.stopPropagation();
    event.preventDefault();
    
    var isUserFavourite = icon.classList.contains("liked");

    var url = isUserFavourite ? '/Favourite/RemoveFromFavourites' : '/Favourite/AddToFavourites';

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(toolId)
    })
    .then(response => {
        if (response.ok) {
            icon.classList.toggle("liked");
            window.location.href = '/';
        } else if (response.status === 401) {
            window.location.href = '/Auth/Login'; 
        } else {
            alert("Something went wrong!");
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert("Error processing the request.");
    });
}