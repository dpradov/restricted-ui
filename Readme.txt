==============================
 Restricted UI (Restricted User Interface) 
==============================
.NET Library for restricting user interface based on security policy

How to control the user interface using a policy established in a declaratively way, based on the user roles and the application status.

Introduction
========
A delicate part in the implementation of applications's security, specially corporate applications, is to determine which functionalities must or not be accessible to the user depending on his role, or which elements should or should not be shown.

It is usual to include in the code logic to hide or disable certain options, buttons, etc, depending on the user accessing the application. This can get complicated when apart from the type of user should also be considered the state of the application, such as the processing status of an entity.
Also, it is quite usual that this policy should be changed with some frequency, in the same way that should must evolve the application requirements. Due to changes in the organizational structure of the company, modifications in management protocols or simply the identification of gaps after the use of the application, among other things, would be necessary to make adjustments to this logic related to the interface.

In response to this problem it is also possible to centralize the definition of this policy, in a common repository for multiple applications. Based on this definition, supported by a particular data model, you can define a library used by the applications that makes it possible to modify the security policy without having to recompile and redeploy applications. The main idea is that the programmer develops as if there were only one type of user, administrator, with permission to do anything. It is the library that controls whether an attempt should be prevented from making visible certain controls or enable them.

This approach is currently used in the company where I work. While offering great flexibility in design, it's concrete implementation presents, from my point of view, a number of constraints and practical difficulties that I have tried to correct with this other library / framework:


Key Features
==========
On the one hand the library / architecture described here does not require to centralize the definition of this security, but it's seen as a case; secondly, to ensure compliance with these interface restrictions it is not necessary to require the programmer to use specific methods or properties of the library, to check if the change is allowed or not, nor has to be controlled by any code; simply any attempt to make visible or enabled any interface element supervised not allowed by the security policy is intercepted and prevented. 

It is allowed to define restrictions policy not only on the basis of roles but also on the application states, in both WinForms and Web applications; and to simplify the definition of this policy a form of maintenace is provided, available both at design time and runtime. 

Notes: 
----------
- It isn't objective of this library to control user authentication. It is assumed that this is done correctly and it is known, with guarantees, the role or roles that the user holds. 
- The name of the library, RestrictedUI refers to the fact that certain elements of the interface will be restricted according to established security ('Restricted area') 
- This library is available in Google Code: http://code.google.com/p/restricted-ui. You can check out the source code from the project's Subversion repository. There you will have documentation and binaries to download, and the posibility to report issues. It is also included a revision documented in spanish 

More information:
http://code.google.com/p/restricted-ui/
