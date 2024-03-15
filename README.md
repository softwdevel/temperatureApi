Readme

use http post method to use this api
http://localhost:5115/api/v1/Temp/getLatest
	
Response body

{
  "avgTemperature": 0,
  "minTemperature": 0,
  "maxTemperature": 0,
  "avgHumidity": 0,
  "minHumidity": 0,
  "maxHumidity": 0
}

where value is type of float

it will return all agregated values of Temperature and Humidity from 20:00 till 08:00 current day if request was done after 08:00, else will return from same time-range but from previous day.
