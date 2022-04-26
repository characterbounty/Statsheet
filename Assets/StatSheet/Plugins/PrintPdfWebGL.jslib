var PrintPdfWebGLPlugin = {
    PrintPdf : function(filenamePtr, width, height, picCount, byteCount, bytesPerPic, allBytes) {
        filename = Pointer_stringify(filenamePtr);
        
        var bpp = new Int32Array(buffer, bytesPerPic, picCount);
        
        var bytes = new Uint8Array(byteCount);
        for (var i = 0; i < byteCount; i++) {
            bytes[i] = HEAPU8[allBytes + i];
        }
        
        PrintPdfImpl(filename, width, height, picCount, bpp, bytes);
    }
};

mergeInto(LibraryManager.library, PrintPdfWebGLPlugin);