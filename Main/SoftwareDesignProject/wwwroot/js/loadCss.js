window.loadCss = (cssFile) => {
    console.log(cssFile)
    let existingLink = document.querySelector(`link[href='${cssFile}']`);
    if (!existingLink) {
        let link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = cssFile;
        document.head.appendChild(link);
    }
};
