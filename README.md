# Google Keyword Scraper
Grabs a list of url from google for specified key words.

#### Usage
Input must have 2-3 arguments:

1.  Keywords (separated by +'s)
2.  Number of results (10, 50, or 100)
3.  Ranking Url (optional; www sensitive)

#### Example Result
Output is a json with a list of urls and a ranking if you provided a ranking url:
```
{
    "results": [
        "http://blah.com",
        "http://blah2.com"
    ]
    "ranking": 1
}
```
