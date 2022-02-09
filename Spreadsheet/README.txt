10/18 -		Added in the code that was provided to us and read through and understood it all. Already dragged and dropped the 'spreadsheet panel' 
		into the spreadsheet GUI then added a label and text box that will let the user know what panel they are currently on
10/19 -		Set up labels and text boxes to where I want them to be and made is so that the 'Cell Name text box' changes when you click on a 
		different cell
10/20 -		Setup a button that sets the value of the current cell based on the contents in the 'content text box'. When the button is pressed the 
		'cell value text box' changes along with the 'spreadsheet panel cell box' that is selected. Does not yet change the value and cell of
		dependents nor does it handle invalid formulas, circular dependencies, or formula errors. Might make a helper method for this!
10/21 -		Setup the 'Set Value' button to change all dependents values and their respective cell when pressed by using a helped method.
		My spreadsheet now has a message box popup that appears when a exception is thrown when changing a cells content, this message box
		prompts the user on what the issue is with their content. Also the spreadsheet displays a "Formula Error" correctly when the cells
		value is a formula error.
			After I did all this I started to work of the menu strip that allows you to Save, Open, create New, and Close spreadsheets along with
		opening the help menu. I implemented the close and open features of the menu strip. 
			I used some outside sources in order to help me figure out how to do some of these features:
		1) Message box help: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.messagebox?view=windowsdesktop-5.0
		2) Save form help: https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-save-files-using-the-savefiledialog-component?view=netframeworkdesktop-4.8
		3) Open form help: https://stackoverflow.com/questions/3965043/how-to-open-a-new-form-from-another-form
10/22 -		Now have features that allow the user to save a spreadsheet file or open a previously saves spreadsheet file. If anything would 
		result in an exception being thrown or if any unsaved changes would be discarded because of an action then the user will be prompted
		with a message telling them the problem. I also commented the code that I currently have plus added some helper methods for
		redundant code.
10/23 -		Implemented my special feature which is a GCD button that calculate the greatest common divisor of two cells that have integer 
		cell values. I also finished commenting my code and creating the help tab for the user.
