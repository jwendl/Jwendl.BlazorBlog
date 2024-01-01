var JsFunctions = window.JsFunctions || {};
JsFunctions = {
    MermaidInitialize: function () {
        mermaid.initialize({
            startOnLoad: true,
            securityLevel: "loose",
            // Other options.
        });
    },

    MermaidRender: function () {
        mermaid.init();
    },

    PrismRender: function () {
        Array.from(document.getElementsByClassName("post-content")).forEach(
            function (element, index, array) {
                Prism.highlightAllUnder(element);
            }
        );
    },
};
