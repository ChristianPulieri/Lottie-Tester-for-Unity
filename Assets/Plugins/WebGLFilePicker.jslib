mergeInto(LibraryManager.library, {
    OpenFilePicker: function (gameObjectNamePtr, callbackMethodNamePtr) {
        var gameObjectName = UTF8ToString(gameObjectNamePtr);
        var callbackMethodName = UTF8ToString(callbackMethodNamePtr);

        var input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json,application/json';

        input.onchange = function(event) {
            var file = event.target.files[0];
            if (!file) {
                return;
            }

            var reader = new FileReader();
            reader.onload = function(e) {
                var jsonContent = e.target.result;

                // Salva in variabile globale per evitare problemi di memoria con grandi stringi
                window.uploadedLottieJSON = jsonContent;

                // Notifica Unity che il file è pronto - usa window.unityInstance o gameInstance globale
                var unityInstance = window.unityInstance || window.gameInstance;
                if (unityInstance) {
                    unityInstance.SendMessage(gameObjectName, callbackMethodName, file.name);
                } else {
                    console.error('Unity instance not found!');
                }
            };

            reader.onerror = function(e) {
                console.error('Error reading file:', e);
            };

            reader.readAsText(file);
        };

        input.click();
    },

    GetUploadedJSON: function () {
        var json = window.uploadedLottieJSON;
        if (!json) {
            return 0;
        }
        var bufferSize = lengthBytesUTF8(json) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(json, buffer, bufferSize);
        return buffer;
    },

    ClearUploadedJSON: function () {
        window.uploadedLottieJSON = null;
    }
});
