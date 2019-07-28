Feature: WikipediaFeature
	In order to get information
	As a general user
	I want to make sure wikipedia is working

@wikipedia
Scenario: Land to Wikipedia and see today's feature
	Given I have navigated to Wikipedia
	When I have selected Wikipedia Main Page tab
	Then I should see the Wikipedia Welcome content
	And I should see the Wikipedia today's feature
