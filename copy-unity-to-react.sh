#!/bin/bash

# Script per copiare il build Unity WebGL in React e applicare fix di performance

set -e

UNITY_BUILD_DIR="Export/ReactBuild"
REACT_PUBLIC_DIR="../Lottie Tester for React/public/unity-build"
TEMPLATE_FILE="../Lottie Tester for React/public/unity-build/index.html"

echo "🚀 Copia build Unity in React..."

# Verifica che il build Unity esista
if [ ! -d "$UNITY_BUILD_DIR" ]; then
    echo "❌ Errore: Build Unity non trovato in $UNITY_BUILD_DIR"
    echo "   Esegui prima il build da Unity: File → Build Settings → Build"
    exit 1
fi

# Salva il template index.html ottimizzato
if [ -f "$TEMPLATE_FILE" ]; then
    echo "💾 Salvo template index.html ottimizzato..."
    cp "$TEMPLATE_FILE" "/tmp/unity-index-optimized.html"
fi

# Crea la directory se non esiste
mkdir -p "$REACT_PUBLIC_DIR"

# Copia tutti i file dal build Unity
rsync -av --delete "$UNITY_BUILD_DIR/" "$REACT_PUBLIC_DIR/"

# Ripristina il template ottimizzato se esiste
if [ -f "/tmp/unity-index-optimized.html" ]; then
    echo "✅ Ripristino template ottimizzato..."
    cp "/tmp/unity-index-optimized.html" "$REACT_PUBLIC_DIR/index.html"
    rm "/tmp/unity-index-optimized.html"
fi

echo "✅ Build copiato e ottimizzato!"
echo ""
echo "📦 Build pronto in: $REACT_PUBLIC_DIR"
echo "💡 Riavvia il server React (npm start) se necessario"
