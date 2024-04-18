# Medical Equipment Procurement System
## Overview
This project implements a web application serving as a centralized information system for companies to procure medical equipment. Private hospitals can use this system to reserve and acquire equipment. Administrators can manage sales reports and oversee various functionalities such as user management, company registration, equipment reservations, and more.

User Types
The system caters to the following user types:

+ Registered User: Employees of private hospitals who can reserve equipment pickup slots, cancel reservations, view scheduled pickup slots, file complaints, and view their order history.
+ Company Administrator: Has access to user profiles, pickup histories, and can manage company profiles.
+ System Administrator: Manages system-level functionalities such as registering companies, managing complaints, defining loyalty programs, and adding other administrators.
+ Unauthenticated Users: Can browse companies and equipment, register, and log in.
## Functional Requirements
The system covers various functionalities including:
+ user registration
+ authentication 
+ user profiles 
+ company profiles 
+ equipment management 
+ pickup scheduling 
+ complaint handling 
+ loyalty programs and more.

For a detailed list of functional requirements, refer to the project specifications.

## Non-Functional Requirements
Email sending functionality can be achieved through external services or by utilizing message queues like RabbitMQ. For clear documentation, the API design adheres to the OpenAPI specification. <br /> <br /> Concurrency issues are effectively managed to ensure smooth user interactions, while integration with location services enables map display and real-time vehicle route tracking. <br />  <br /> To address scalability concerns, proposed strategies include caching, partitioning, replication, load balancing, and monitoring.

Regarding simulator integration, in the main application, location data is forwarded to Azure Functions acting as simulators for real-time vehicle tracking. Within these Azure Functions, the Google Maps API is utilized to generate routes based on the received coordinates, outlining the path for simulated delivery vehicles. Subsequently, these updated coordinates are asynchronously transmitted back to the main system via RabbitMQ. <br /> <br /> The main application then listens for these coordinates and visually represents the simulated vehicle routes for equipment delivery.

For a more detailed overview of the project and its functionalities, please refer to the comprehensive specifications document.



