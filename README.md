# SwarmAI
This is a project for the 2019-2020 ATOS challenge on Collaborative AI. Our submission was based on creating agents which are able to collaborate with each other to search a building. The use case was of buildings in a natural disaster where traditonal infrastructure could be down so a local solution is needed. 

Challenge Link: https://www.atositchallenge.net/2020-theme-cooperative-artificial-intelligence/

* A soultion was porduced using reinforcement learning.
* Using Unity Engien as a testing and training enviroment to create AI.
* Mulitple different AI was created to find the best possible network configuration.
[![](http://img.youtube.com/vi/dIc1bhqJ3ZE/0.jpg)](http://www.youtube.com/watch?v=dIc1bhqJ3ZE "")

## Process
1. Data exploration was done to understand the dataset and the type of data being used.
2. Data transformation was done over the entire dataset so that it can be successfully pipelined into the proposed machine learning model.
3. The proposed model has many layers and inputs; it has two embeddings layers which take in different features and have an LSTM at the end of them. 
The results of the two LSTMs are combined with additional data and feed into multiple dense layers to predict a final value.
4. The results have been transformed so that they can be uploaded for submission.

## Libraries used

Python Libraries:
* Numpy
* Pandas
* Sci-Kit Learn
* Keras
