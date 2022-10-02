
export default {
	install(app){
		const validationManager = {
			errors : {
				messages : {},
				reset : function(){
					for(let i in this.messages){
						this.messages[i] = [];
					}
				}
			}	,
			validationInvalid : function(){
				for(let i in this.errors.messages){
					if( this.errors.messages[i].length > 0) return true;
				}
				return false;
			},
			regexes: {
				email : /^(?=.{5,50}$)[a-zA-ZšđžćčŠĐŽĆČ0-9.!#$%&'*+/=? ^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/,
				password :/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/,
				firstOrLastname : /^[A-ZŠĐŽĆČ][a-zšđžćč]{1,14}(\s[A-ZŠĐŽĆČ][a-zšđžćč]{1,14}){0,1}$/,
				username : /^(?=.{3,20}$)[a-zA-Z0-9._]+$/,
				category : /^[A-Z][a-z]{2,19}$/,
				route : /^\/[a-z\s]{3,20}$/,
				usecase : /^[A-Za-z]{5,60}$/
			}
		}
		app.provide("validationManager", validationManager);
	}
}