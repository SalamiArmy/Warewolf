@Deploy
Feature: Deploy Feature
In order to schedule workflows
	As a Warewolf user
	I want to setup schedules

Scenario: Create and Deploy a renamed resource to localhost
	Given I am Connected to remote server "tst-ci-remote"
	And I have a workflow "OriginalName"
	And "OriginalName" contains an Assign "Rec To Convert" as
	| variable    | value |
	| [[rec().a]] | yes   |
	| [[rec().a]] | no    |
	And "OriginalName" contains Count Record "CountRec" on "[[rec()]]" into "[[count]]"
	When "OriginalName" is executed
	And I select and deploy resource from source server
	When I rename "OriginalName" to "RenamedResource" and re deploy
	And I reload the source resources
	And I select and deploy resource from source server 
	And I reload the destination resources
	Then Destination server has new workflow name