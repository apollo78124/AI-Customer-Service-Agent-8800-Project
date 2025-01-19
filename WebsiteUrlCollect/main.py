import requests
from bs4 import BeautifulSoup
import time

# Function to fetch HTML content from a URL
def fetch_html(url):
    try:
        response = requests.get(url)
        if response.status_code == 200:
            return response.text
        return None
    except requests.RequestException as e:
        print(f"Error fetching {url}: {e}")
        return None

# Function to extract internal URLs from a page's HTML
def extract_urls(base_url, html_content):
    soup = BeautifulSoup(html_content, 'html.parser')
    links = set()  # To avoid duplicate links
    for link in soup.find_all('a', href=True):
        href = link['href']
        if href.startswith('/'):  # Convert relative URL to absolute
            href = base_url + href
        if base_url in href:  # Only consider internal URLs
            links.add(href)
    return links

# Crawl website and collect URLs
def crawl_website(base_url):
    crawled_urls = set()
    to_crawl = {base_url}
    all_urls = set()
    page_count = 0

    print(f"Starting crawl for: {base_url}")
    start_time = time.time()

    while to_crawl:
        url = to_crawl.pop()
        if url not in crawled_urls:
            print(f"Crawling {url} ...")
            html_content = fetch_html(url)
            if html_content:
                crawled_urls.add(url)
                page_count += 1
                new_urls = extract_urls(base_url, html_content)
                all_urls.update(new_urls)
                to_crawl.update(new_urls)

                # Console update after each crawl
                print(f"Page crawled: {page_count}")
                print(f"URLs found so far: {len(all_urls)}")
                print(f"URLs to crawl: {len(to_crawl)}")
            else:
                print(f"Failed to crawl {url}")

    end_time = time.time()
    print(f"Finished crawling {page_count} pages in {round(end_time - start_time, 2)} seconds.")
    return all_urls

# Save the extracted URLs to a txt file
def save_urls_to_file(urls, file_name):
    with open(file_name, 'w') as f:
        for url in sorted(urls):
            f.write(url + '\n')
    print(f"Saved {len(urls)} URLs to '{file_name}'.")

# Example usage
if __name__ == '__main__':
    base_url = 'https://www.bcit.ca'  # Replace with the target website's base URL
    urls = crawl_website(base_url)
    save_urls_to_file(urls, './extracted_urls.txt')
    print(f"Process complete! A total of {len(urls)} URLs were saved to './extracted_urls.txt'.")
