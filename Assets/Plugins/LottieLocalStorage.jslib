mergeInto(LibraryManager.library, {
    GetLottieJSON: function () {
        var json = localStorage.getItem('lottieJSON');
        if (json === null || json === undefined || json === '') {
            return 0;
        }
        var bufferSize = lengthBytesUTF8(json) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(json, buffer, bufferSize);
        return buffer;
    },

    GetLottieHash: function () {
        var hash = localStorage.getItem('lottieHash');
        if (hash === null || hash === undefined || hash === '') {
            return 0;
        }
        var bufferSize = lengthBytesUTF8(hash) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(hash, buffer, bufferSize);
        return buffer;
    }
});
