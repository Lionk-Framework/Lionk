# User Stories for the .NET Plugin Architecture Project

## User: End User (Operator)

**User Story 1: Data Consultation**
- As an end user, I want to be able to view data specific to the component used from a continuously updated dashboard.

**User Story 2: Alarm Management**
As an end user, I want:
- to be notified when there is a system failure or a component failure.
- to be notified when a notification is raised within one of the components, for example, if the component allows me to be notified when the temperature of a room exceeds a certain threshold.
- all these notifications/alarms to be displayed regardless of the page used.
- to be notified by email/push notification.

**User Story 3: Application Access**
- As an end user, I want to be able to access the application remotely via a web browser.

## User: End User (Administrator) inherits all operator needs
**User Story 4: Application Configuration**
As an end user, I want:
- to be able to configure the application according to my needs, by adding or removing components.
- to be able to configure components according to the parameters they offer.
- to be able to link components together.

**User Story 5: Plugin Integration**
As an end user, I want:
- to be able to integrate third-party plugins into the application.
- to be able to enable/disable plugins.

**User Story 5: Component management**
As an end user, I want:
- to be able to configure the behavior of components.
- to be able to view the status of components.
- to be able to configure alarms and notifications for components.
- to be able to configure the data collection frequency for components.
- to be able to add or delete components.

**User Story 6: User Management**
As an end user, I want:
- to be able to add/remove users.
- to be able to assign roles to these users.

**User Story 7: Data Management**
As an end user, I want:
- to be able to export data collected by a component by choosing the output file name in JSON format.
- to be able to delete data collected by a component.

## User: Plugin Developer
**User Story 8: Plugin Development**
As a plugin developer, I want:
- to access complete documentation and code examples to create new plugins compatible with the system.
- to be able to access the SDK to develop plugins via NuGet packages.

**User Story 9: Plugin Deployment**
As a plugin developer, I want:
- to be able to submit my plugins to the administrator for integration into the system as `dll`.

# User Stories for the Plugin PoC for Component Integration for Boiler Control

## User: End User (Operator)
**User Story 1:**
As an end user, I want:
- to be able to retrieve the energy produced by an accumulation fireplace and store it.
- the heat flow to be optimized based on storage temperatures via a three-way valve.
- the system to be able to self-regulate based on the temperatures of the living room fireplace.
- to receive a notification if the system fails.
- to receive an alarm if the storage temperature exceeds a certain threshold.
- to receive an alarm if the temperature of the living room fireplace exceeds a certain threshold.
- to know the amount of energy stored to determine if it is appropriate to light the living room fireplace.
