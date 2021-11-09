# LiteChat

This is a real-time-chat application made with Angular so the users from [DeemZ](https://github.com/Berat-Dzhevdetov/DeemZ-Platform/tree/main/DeemZ)

# 🛠 Built with:

- Angular
- Firebase Firestore Database
- CKEditor
- Material loader
- Material progress bar
- Angular animations
- MomentJS
- DeemZ Backend
- Custom scroll bar

# Permissions:

| **Permissions**      | Guest | Logged in User | Admin |
| -------------------- | ----- | -------------- | ----- |
| View questions       | ✅    | ✅             | ✅    |
| Ask questions        | ❌    | ✅             | ✅    |
| Write replies        | ❌    | ✅             | ✅    |
| Like questions       | ❌    | ✅             | ✅    |
| Delete question      | ❌    | ❌             | ✅    |
| Purge all questions  | ❌    | ❌             | ✅    |
| Change theme         | ✅    | ✅             | ✅    |
| Change message state | ✅    | ✅             | ✅    |

# Pages:

**Home page**:

Here all users can view the questions and the logged ones can write ask new questions and replies and also like already submited questions by the other users.
![Home page](https://user-images.githubusercontent.com/56674380/140882095-8c04a264-30a8-4e69-96f7-d6abf241ebf8.png)


**Replies section**:

Every question can have replies from the other users. All users can view these replies but only the logged in users can send other replies. This section has a custom scroll bar.
![Replies](https://user-images.githubusercontent.com/56674380/140882225-9a85ee2b-6498-4beb-9901-441c25c1d426.png)

**User options section**:

From this menu, the user can change presonal preferences such as the order of the messages and the color of the color theme.
![Options](https://user-images.githubusercontent.com/56674380/140882315-c878588c-04bd-43f2-ba15-052711a07aa8.png)

Here we can see the application with the white theme:
![Light theme](https://user-images.githubusercontent.com/56674380/140882417-ed7ad53c-4b3d-4c56-8bef-abd44a804d82.png)


**Custom loader**:

This is the Material loader the admin is shown when they are bulk deleting all messeges in that course.
![Custom loader](https://user-images.githubusercontent.com/56674380/140882503-643c53cf-1e51-4bf3-98a8-1079faa4bbbb.png)

