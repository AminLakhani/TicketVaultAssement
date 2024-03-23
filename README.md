## Instructions

As a technical lead on your team it’s your job to show the developer who wrote this code what can be improved – everything from 
- code structure
- the code itself 
- ideas for improved architecture of the entire solution
- logical bugs which may need to be fixed

Make at least the most critical improvements to the code as needed. 
If you’re inclined to show all of your ideas in code, change however much you require.
Provide a summary of your code changes as well as future improvement ideas for the developer.

## Notes

**Issue: Slow & Crashes**\
**Cause(s):** Slow is probably because it was pulling all the data every run. Similarly because it's pulling all the data every run will cause throttling + timeout crashes since there is no error handling in application.\
**Solution:** Updated app to only get data it needs, added basic error handling so if it crashes we'll at least know why.\

**Issue**: Missing Data\
**Cause(s)**: Not getting 31st day of any month that has that day since day was hardcoded to 30. Missing sentitment score and day of sentiment analysis as well.\
**Solution**: Changed iteration logic so it loops through days and not hard coded values, added missing fields.\

**Issue**: DB Larger Than Expected\
**Cause**: App was not checking if the record it pulled from the API already exsisted, was adding duplicate data to db every time it was run.\
**Solution**: Wrote data for each day out to a file and checked to see if file existed before processing a day. It should probably check each record but that seems like overkill for this assesment.\


Changes Made:
  - Changed tupples to more logical c# models. Will help code readability, maintainability, etc..
  - Added Date to ticker record so we know on which date this data is for as well as Sentiment Score because we might as well save all fields from API if we're going through the trouble of getting the data.
  - Writing data out to drive so that we can run a check if we've already pulled that data.
  - Added (very) basic error handling.
  - Parameterize/config api url, dates, filepath, etc..
  - Seperate concerns - API call and read/write to file/"db" should be handled by seperate classes, this will also make unit tests possible.

## TO DO:
- Add logging + better more specific error handling.
