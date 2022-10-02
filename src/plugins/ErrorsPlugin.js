export default {
	install(app){
		app.provide("errorMessages", {
			email : "Letters, numbers and special characters such as '.!#$%&'*+/=? ^_`{|}~-' are allowed. Length is between 5 and 50.",
			password : "At least one character and one number with 8 characters minimally.",
			firstOrLastname : "First letter of a word is capital with  only one space between words.Minimum characters 2, maximum 29. ",
			username : "Username can take letters, numbers and special characters such as . and _. Length is between 3 and 20.",
			category : "First letter is capital, no spaces, no numbers. Length  is between 2 and 19.",
			route : "Lowercase letters. Length is between 3 and 20.",
			usecase : "Only letters are acceptable. Names length needs to be between 5 and 60 characters."
		})
	}
}