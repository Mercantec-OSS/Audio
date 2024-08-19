// Event listener for file input change
document.getElementById('fileInput').addEventListener('change', function (event) {
    var files = event.target.files; // Get the selected files
    for (var i = 0; i < files.length; i++) { // Loop through each selected file
        var file = files[i];
        var url = URL.createObjectURL(file); // Create a URL for the file
        createVisualizer(url); // Call function to create the visualizer for each file
    }
});

// Function to create the audio visualizer
function createVisualizer(audioSrc) {
    // Create a container div for audio and visualizer
    var container = document.createElement('div');
    container.classList.add('audio-container');

    // Create an audio element and set its source
    var audio = document.createElement('audio');
    audio.src = audioSrc;

    // Append audio element to the container
    container.appendChild(audio);

    // Append the container to the visualizers div
    document.getElementById('visualizers').appendChild(container);

    // Create a div for the waveform visualizer
    var waveformContainer = document.createElement('div');
    waveformContainer.classList.add('waveform');
    container.appendChild(waveformContainer);

    // Import WaveSurfer.js module and initialize the waveform visualizer
    import('https://unpkg.com/wavesurfer.js@7/dist/wavesurfer.esm.js').then(module => {
        const WaveSurfer = module.default;
        const wavesurfer = WaveSurfer.create({
            container: waveformContainer, // Container for the waveform
            waveColor: "Black", // Waveform color
            progressColor: "Pink", // Progress color
            backend: 'MediaElement', // Backend to link with the audio element
            mediaControls: true, // Show native media controls
            audioRate: 1, // Playback speed
            barWidth: 1, // Bar width for the waveform
            barHeight: 1, // Bar height for the waveform
            normalize: true, // Normalize the waveform
            url: audioSrc, // Audio source URL
        });

        // Play the audio when user interacts with the waveform
        wavesurfer.on('interaction', () => {
            wavesurfer.play();
        });
    });

    // Create a div for audio controls
    var audioControls = document.createElement('div');
    audioControls.classList.add('audio-controls');

    // Create and style a title div
    var titleDiv = document.createElement('div');
    titleDiv.classList.add('title-div');

    // Create a title element, make it editable, and append to the title div
    var title = document.createElement("td");
    title.textContent = "Sound of the best TWELl";
    title.classList.add("title");
    title.contentEditable = true;
    titleDiv.appendChild(title);

    // Append title div to audio controls
    audioControls.appendChild(titleDiv);

    // Create a div for additional controls
    var controlsDiv = document.createElement('div');
    controlsDiv.classList.add('controls-div');

    // Create and style a description element, make it editable, and append to controls
    var description = document.createElement("div");
    description.textContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.  ";
    description.classList.add("description");
    description.contentEditable = true;
    audioControls.appendChild(description);

    // Create and style tags element, make it editable, and append to controls
    var tags = document.createElement("div");
    tags.textContent = "Tags";
    tags.classList.add("tags");
    tags.contentEditable = true;
    audioControls.appendChild(tags);

    // Create and style category element, make it editable, and append to controls
    var category = document.createElement("div");
    category.textContent = "DragonLord, Astra, Duelist";
    category.classList.add("category");
    category.contentEditable = true;
    audioControls.appendChild(category);

    // Create a download button, set its properties, and append to controls
    var downloadButton = document.createElement('a');
    downloadButton.textContent = 'Download';
    downloadButton.download = 'audio.mp3'; // Set download filename
    downloadButton.href = audioSrc; // Set href to audio source URL
    downloadButton.classList.add('download-button'); // Add class for styling
    audioControls.appendChild(downloadButton);

    // Create an image element for Creative Commons logo
    var img = document.createElement('img');
    img.src = 'images/cc.png'; // Image source
    img.alt = 'Description of the image'; // Alt text for the image
    img.classList.add("logo"); // Add class for styling

    // Create an anchor element for the image, set its href, and append the image
    var anchor = document.createElement('a');
    anchor.href = "https://creativecommons.org/licenses/by-nc/4.0/deed.en"; // Link to Creative Commons license
    anchor.appendChild(img);

    // Append the anchor to audio controls
    audioControls.appendChild(anchor);
    audioControls.appendChild(controlsDiv); // Append controls div to audio controls

    // Append the entire audio controls to the container
    container.appendChild(audioControls);
}

// Form submission event listener to validate inputs
document.getElementById("myForm").addEventListener("submit", function (event) {
    const title = document.getElementById('title').value.trim(); // Get trimmed title value
    const tags = document.getElementById('tags').value.trim(); // Get trimmed tags value
    const description = document.getElementById('description').value.trim(); // Get trimmed description value
    const checkbox = document.getElementById('check-24').checked; // Check if checkbox is checked
    const file = document.getElementById('fileInput').files.length > 0; // Check if a file is selected

    // Validate all fields, prevent form submission if any field is empty or unchecked
    if (!file || !title || !tags || !description || !checkbox) {
        alert("Udfyld alle felterne"); // Alert user to fill all fields
        event.preventDefault(); // Prevent form submission
    } else {
        alert("Filen er blevet sendt til Admin køen og venter på at blive godkendt, før den kan blive uploadet."); // Confirmation message
    }
});

// Event listener for DOM content loaded to set active button based on the current path
document.addEventListener("DOMContentLoaded", function () {
    var path = window.location.pathname; // Get the current URL path

    // Add 'active' class to the appropriate button based on the path
    if (path.includes("UploadPage")) {
        document.getElementById("uploadBtn").classList.add("active"); // Highlight Upload button
    } else if (path.includes("LoginPage")) {
        document.getElementById("loginBtn").classList.add("active"); // Highlight Login button
    } else if (path === "/" || path.includes("Index")) {
        // Optionally handle the Index page if needed
    }
});
