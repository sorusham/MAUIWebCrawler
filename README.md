<h1 align="center">MAUI Web Crawler</h1>

<h2>Overview</h2>
<p>A lightweight <strong>web crawler</strong> built with MAUI, focused on extracting <strong>images</strong> and <strong>URLs</strong> from a given webpage. This tool is simple yet effective, offering targeted extraction without additional settings or limitations.</p>

<h2>Features</h2>
<ul>
    <li><strong>Extracts only links and images</strong> from a specified URL</li>
    <li>Built with <strong>MAUI</strong> for cross-platform compatibility</li>
    <li>No complex configuration — just set the URL, and the crawler handles the rest</li>
</ul>

<h2>Getting Started</h2>

<h3>Prerequisites</h3>
<ul>
    <li><strong>MAUI SDK</strong> installed</li>
    <li><strong>.NET 6 or higher</strong></li>
</ul>

<h3>Installation</h3>
<p>To set up the project on your local machine:</p>
<pre>
<code>
git clone https://github.com/SorushPro/MAUIWebCrawler.git
cd MAUIWebCrawler
</code>
</pre>
<p>Then, run the crawler:</p>
<pre>
<code>
dotnet run
</code>
</pre>

<h3>Usage</h3>
<p>Simply provide the target URL. The crawler will:</p>
<ol>
    <li>Scan the webpage</li>
    <li>Extract all links and images</li>
    <li>Display or save the extracted data for easy access</li>
</ol>

<h3>Example Output</h3>
<p>Here’s a sample output to illustrate the crawler's functionality:</p>
<pre>
<code>
Images Found:
- https://example.com/image1.jpg
- https://example.com/image2.jpg

Links Found:
- https://example.com/page1
- https://example.com/page2
</code>
</pre>


<h2>License</h2>
<p>This project is open-source and distributed under the Apache License. See the <a href="LICENSE">LICENSE</a> file for more details.</p>


