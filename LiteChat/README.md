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
![HomePage](https://i.ibb.co/5czLqvN/home-black.png)

**Replies section**:

Every question can have replies from the other users. All users can view these replies but only the logged in users can send other replies. This section has a custom scroll bar.
![Replies](https://i.ibb.co/1q7jZq5/replies.png)

**User options section**:

From this menu, the user can change presonal preferences such as the order of the messages and the color of the color theme.
![Options](https://i.ibb.co/RgBNpyh/options.png)

Here we can see the application with the white theme:
![Light theme](https://i.ibb.co/VYw66Sc/light-theme.png)

**Custom loader**:

This is the Material loader the admin is shown when they are bulk deleting all messeges in that course.
![CustomLoader](https://i.ibb.co/qJwDR2D/laoder.png)
