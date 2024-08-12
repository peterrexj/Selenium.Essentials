Feature: WikipediaFeature
	In order to get information
	As a general user
	I want to make sure wikipedia is working

@wikipedia
Scenario Outline: Navigate to Wikipedia and verify today's feature
	Given I open a new browser of type <Browser>
	And I have navigated to Wikipedia
	When I have selected Wikipedia Main Page tab
	Then I should see the Wikipedia Welcome content
	And I should see the Wikipedia today's feature

Examples:
	| Browser             |
	| Win11_Chrome_Latest |
	| MacOS_Ventura_Chrome_Latest |