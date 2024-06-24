# Plan

At the end, we want a wizard which takes in:
    - A text
    - The language of the text
    - A Language Level to be selected 
        - As either realistic or extensive reading
        - To Kazakov's Levels 
    - Generate a LaTeX Document
    - Return it 

How it will be handled:
    - A form which has the file upload, and two dropdowns 
    - A controller which directs it into an object 
    - Then get the words for the language above the level 
    - Then generate the gloss for each word
        - This should either be from a database or from the Wiktionary API
    - Then, correct the text with the LaTeX glosses 
    - Then, insert it into a draft LaTeX document 
    - Return that LaTeX document