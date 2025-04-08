﻿using ToolInterface;

namespace JSONKeyExtractor;

public class JSONKeyExtractor : ITool
{
    public string Name => "JSON Key Extractor";
    public string Category => "Data";
    public string Description => "Extract all unique keys from your JSON input.";

    public object Execute(object? input) => null;

    public string GetUI() => @"
<div class='container py-5 mx-auto' style='max-width: 700px;'>
    <!-- Header -->
    <div class='header mb-4'>
        <h1 class='text-start m-0'>JSON Key Extractor</h1>
        <div class='separator my-2' style='width: 350px; height: 1.5px; opacity: 0.3; background: #a1a1a1'></div>
        <p class='text-start text-muted mb-0'>Extract all unique keys from your JSON input.</p>
    </div>

    <!-- Card content -->
    <div class='card shadow p-4'>
        <div class='mb-3'>
            <label for='jsonInput' class='form-label'>JSON Input</label>
            <textarea id='jsonInput' class='form-control' rows='6' placeholder='Paste your JSON here'></textarea>
        </div>

        <button onclick='extractKeys()' class='btn btn-primary mb-3'>Extract Keys</button>

        <div class='mb-2 d-flex justify-content-between align-items-center'>
            <label for='keysOutput' class='form-label mb-0'>Extracted Keys</label>
            <button onclick='copyKeys()' class='btn btn-sm btn-outline-secondary'>
                <i class='bi bi-copy'></i>
                Copy
            </button>
        </div>

        <textarea id='keysOutput' class='form-control' rows='6' readonly placeholder='List of keys will appear here'></textarea>
    </div>
</div>

<script>
    function extractKeys() {
        const jsonStr = document.getElementById('jsonInput').value.trim();
        const output = document.getElementById('keysOutput');

        if (!jsonStr) {
            output.value = 'Input is empty';
            return;
        }

        try {
            const obj = JSON.parse(jsonStr);
            const keysSet = new Set();
            const resultLines = [];

            function traverse(value, depth = 0) {
                if (Array.isArray(value)) {
                    value.forEach(item => traverse(item, depth));
                } else if (value !== null && typeof value === 'object') {
                    for (const key in value) {
                        const indent = '\t'.repeat(depth);
                        if (!keysSet.has(key)) {
                            resultLines.push(indent + key);
                            keysSet.add(key);
                        }
                        traverse(value[key], depth + 1);
                    }
                }
            }

            traverse(obj);
            output.value = resultLines.join('\n');
        } catch (e) {
            output.value = 'Invalid JSON format';
        }
    }

    function copyKeys() {
        const output = document.getElementById('keysOutput');
        output.select();
        output.setSelectionRange(0, 99999);
        document.execCommand('copy');
    }
</script>

";
    public void Stop() { }
}
