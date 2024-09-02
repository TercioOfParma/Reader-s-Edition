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
    - A form which has the file upload, and two dropdowns X
    - A controller which directs it into an object X
    - Then get the words for the language above the level X 
    - Then generate the gloss for each word
        - This should either be from a database or from the Wiktionary API
    - Then, correct the text with the LaTeX glosses 
    - Then, insert it into a draft LaTeX document 
    - Return that LaTeX document


    To do list:
    - Take in a text and load all of the words in for the appropriate languages
    - Take in a text in a certain text language, and return a table of words with selections of the glosses for those words
    - Add in a scaffold for that based on the frequency of the word in the text



    How to take in a text and gloss it accurately?
    1) Load only the words that pass the threshold
    2) Load those from the database
    3) Load the definitions into each instance of the world
    4) Return to the Frontend