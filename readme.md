Developed for CIBC Contract Example of Clean Code. August 2019
by Darren King

Requirement From CIBC Capital Markets
=====================================
Coding Task

Complete the task using any version of VisualStudio and target any version of the .Net Framework, .Net Core or .Net Standard.

Create an application that displays simulated stock prices ticking (price for each stock is provided below)
Stock1 - random price between 240 and 270
Stock2 - random price between 180 and 210

Requirements

1.	The project should be separated into Client module and Service module (Assemblies).  Client module could be WPF, WinForm or Console application and will display live ticking prices. No hard requirements in displaying the price ticks.
2.	We would like to keep track of all the price ticks along with the time that the price changed within the client (in memory). 
3.	Client should only subscribe with a ticker i.e. Stock1, and the service will keep publishing prices for that ticker for as long as it is subscribed.
4.	Service module will act as a provider and publish prices based on the price logic to the client.
5.	Design the project in such a way that the client module can use different implementations of Service module without changing the client code (loading during runtime).


Note: For this project do not use any type of datastore/database/WebAPI/WebService.  Service should be in memory provider.


Description
===========
Clean code can mean a few things. This application required an Interface Kernel in order to keep things clean.

The Service Layer is loaded in dynamically using the Autofac IoC. This library also required a stock price producer which is a simulation
The interfaces are exactly the same but in a full production application they would not be since they actually do different things
logically. However, for this demo there isn't any difference for that reason.

This simple project resulted in a contract offer.