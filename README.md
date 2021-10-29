# WJS_Movie_Library

.Net Database Class Movie Library Assignments Repository.  

This Repository will contain all the class assignments that will be building upon the Movie Library concept.

There will be branches for each Assignment..

A4 - Class 04 Assignment

	We are going to build a Movie library application

	Download initial movie data file - you will only need to use the movies.csv

	Create Movie Console Application

	1. List all movies in the file
		Note: Do not just scroll all movies - be smart about how to display (is there a NuGet package that could help?)

	2. Allow adding movies to the file
		Do not enter duplicate IDs.  Determine how to best figure out the next/appropriate movieId ("identity") value to use
		Do not allow duplicate movie titles to be entered (exact string match is ok)

	Implement with the following features in mind
		Exception Handling
		NLog framework
		Consider creation of additional classes/methods
		Unit test (at least one test)


A6 - Class 06 Assignment

	Modify your Movie application - add support for different Movie Types

	Movie
		int id (1)
		string title (Toy Story)
		string[] genres (Action, Horror)

	Show 
		int id (1)
		string title (Supernatural)
		int season (2)
		int episode (12)
		string[] writers (Kripke)

	Video
		int id (1)
		string title (Lethal Weapon 3)
		string format (VHS, DVD, BluRay)
		int length (100)
		int[] regions (0,2)

	You DO NOT need to use the entire movies.csv (use maybe 10 entries)

	Create shows.csv and videos.csv using the format above

	These should be added through the use of abstract classes as demonstrated in class.  

	Add an abstract Display() method to each class that will output the content appropriately.

	1. Ask the user which Media type to display

	* example: Media media = new Show();

	2. Display the specific contents of the file

	* example: media.Display();


A7 - Class 07 Assignment

	Modify your existing Movie Library program to support the use of another "repository" implemented via Interfaces.  
	Specifically we currently support a file based "repository", meaning all data is read from and written to a file.

	Your updated program should be modified to include a JSON repository where the data is stored and written in the JSON format.
	Using interfaces as discussed in class, this should require minimal changes to your actual code outside of modifications to 
	support the interface.

	Functionally your program will work as it has in the past to read and write movie data, but the file used will be the movies.json file.

	Note: Use the JSON.NET library as demonstrated in class to make your life much easier.


A9 - Class 09 Assignment
	Only an extension to your existing application - Do Not Create a Database

	Add search to the Movie application across all "Movie" types

		User should be able to perform a search based on title
			Using LINQ - create a substring search, case insensitive

		The search results should display the results and the number of matches

		One single search should return results from all media types

		Extra credit: Identify the type of movie (show, video, movie)

