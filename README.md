# Tourist Agency Information System

This project is a web application that simulates an **online tourist agency system**, developed as part of the **Web Programming** course at **Applied Software Engineering 2024/2025**.

## About the Project

The goal of the project is to design and implement an information system that enables travel reservations and provides information about tourist destinations. The application is used by three main user groups (roles): **Tourist**, **Manager**, and **Administrator**.

---

##  Key Functionalities

The application supports the following roles and functionalities:

###  Unregistered User
* **Arrangement Overview:** Can view, search, and sort all **upcoming arrangements** (from the soonest to the furthest) on the homepage.
    * **Search Criteria:** Lower/upper date bounds for start/end of the trip, transport type, arrangement type, and name.
    * **Sorting:** By name, trip start date, and trip end date (ascending and descending).
* **Details:** Can view detailed information for each arrangement, which includes accommodation details (and comments) and the status of available/occupied accommodation units.
    * When viewing details, search and sorting are available for accommodations and accommodation units.
* **Registration:** Registers on the application by filling in the required fields and automatically becomes a **Tourist**.
* **Login:** Logs into the system by entering a username and password.

### Logged-in User
* The homepage displays the same content as for an unregistered user.
* **Profile:** Can view and edit their profile.
* **Navigation:** Can access pages corresponding to their specific role.

### Tourist
* **Reservation:** Can reserve an arrangement by finding a free accommodation unit within the selected accommodation.
* **Cancellation:** Can cancel a reservation, which frees up the accommodation unit (cancellation is not allowed if the arrangement has already passed).
* **Reservation Review:** Can view all of their reservations (future and previous; active and cancelled).
    * **Search/Sorting:** By unique identifier, arrangement name, and status.
* **Commenting:** After an arrangement has passed, the Tourist can leave a comment on the accommodation. The comment is initially only visible to the Manager who created the arrangement.

### Manager
* **Arrangement Management:** Creates, modifies, views, and logically deletes their own arrangements.
    * Deletion of an arrangement is **not allowed** if there is an active reservation for it.
* **Accommodation/Unit Management:** Creates, modifies, views, and logically deletes accommodations and accommodation units within accommodations.
    * Deletion of accommodation/units and modification of the number of beds have restrictions based on existing future reservations.
* **Reservation Review:** Views all reservations for their arrangements and can see the details of each reservation.
* **Comment Approval:** Can see all comments for their arrangements.
    * Can **approve** (becomes visible to everyone) or **reject** (visible only to the Manager) comments. They can only manage comments for arrangements they created.

### Administrator
* **Users Overview:** Has an overview of all system users.
    * **Search/Filter:** Can search users by name and surname and filter by role.
* **Manager Registration:** Registers new Managers.
* **Initial Accounts:** Administrator accounts are loaded programmatically from a text file and cannot be added later.

### ℹ️ Important Note on Search Functionality
* **Combined Search:** Every search must be of a combined nature; the user enters values for multiple search parameters, and the results must satisfy **every** search criterion.
* **Textual Search:** For text fields, the search should check if the attribute value **contains** the entered value (e.g., searching for "di" in the arrangement name should find "Divčibare zimovanje," "Leto na Maldivima," etc.).
