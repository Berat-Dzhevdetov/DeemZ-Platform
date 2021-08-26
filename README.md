# DeemZ

![image](https://user-images.githubusercontent.com/56674380/129486206-6f0a40c7-fe25-46cd-8b84-933e5e3d2532.png)

This project is made with ASP.NET Core 5. The design is taken from [SoftUni](https://softuni.bg/) for educational purposes! Don't forget to make initial migrate if you want to start the app.

ASP.NET Core web application for online programming learing where you can take exams after the course and receive points.

## 🛠 Built with:
- ASP.NET Core MVC
- MS SQL Server
- Cloudinary
- Font-awesome
- Bootstrap
- SignalR

## Permissions:
Permission | Guest | Logged User | Admin
-- | ---- | ---- | ---
Index page | ✅ | ✅ | ✅
Privacy page | ✅ | ✅ | ✅
Forum | ✅ | ✅ | ✅
View Course Details | ✅ | ✅ | ✅
Add report to resource | ❌ | ✅ | ✅
Sign up for the course by paying | ❌ | ✅ | ❌
View Course Resources | ❌ | ✅ (only if the user has paid for the course)| ✅
Download Course Resources | ❌ | ✅ (only if the user has paid for the course)| ✅
Admin Dashboard  | ❌ | ❌ | ✅
Add Course  | ❌ | ❌ | ✅
Edit Course  | ❌ | ❌ | ✅
Delete Course  | ❌ | ❌ | ✅
Add Lecture to Course  | ❌ | ❌ | ✅
Add Exam to Course  | ❌ | ❌ | ✅
Edit Exam  | ❌ | ❌ | ✅
Delete Exam  | ❌ | ❌ | ✅
Edit Lecture  | ❌ | ❌ | ✅
Delete Lecture  | ❌ | ❌ | ✅
Upload Resource to Lecture  | ❌ | ❌ | ✅
Delete Resource | ❌ | ❌ | ✅
Edit User | ❌ | ❌ | ✅
Sign Up User to Course (basically for adding lecturer to the course) | ❌ | ❌ | ✅
Remove User From Course | ❌ | ❌ | ✅
Delete report | ❌ | ❌ | ✅

## Pages:

### Public Pages:

**Home Page**

This is the landing page of the application, from here you can read infromation about the company.
![image](https://user-images.githubusercontent.com/56674380/129486952-c0732410-b630-4eab-98b8-f5e451f72315.png)

**Forum Topics**

In this page, all written topics are displayed, here you can get brief information about the topic. You can also search topic by title using the search bar on the top of the page.
![image](https://user-images.githubusercontent.com/56674380/129487234-70a9fea8-2a4b-462a-985b-04934093adc2.png)

**Course information**

On this page you can see information about the course such as when it starts, what will be studied during the course and etc.
![image](https://user-images.githubusercontent.com/56674380/129883919-887da01f-693e-4d3d-8e35-183bbe054889.png)


### Pages for Logged Users:
**Posting a Topic**

From this page, you can create a new topic. After choosing an appropriate title and description you can click the button Create in the bottom of the form.
![image](https://user-images.githubusercontent.com/56674380/129487333-1704af2b-e5b5-4921-b5c9-8ab6dbab1362.png)

**Course resources**

When you click on course's resource window will appear in which you will be able to view the resource only if you are admin or if you are signed up for the course! If the link is to another site (Facebook, YouTube) a new window will open in your browser.

![image](https://user-images.githubusercontent.com/56674380/129884999-5253563c-09da-4f29-b819-defcb18193cb.png)

**Report issue with course's resource or lecture**

If you find any issue with the course's resource or lecture you can describe your problem which will be directly send to admin for a look. Every resource have link under it for report.

![image](https://user-images.githubusercontent.com/56674380/129885651-9abf83b5-94fd-45db-8789-9a413ca89124.png)

![image](https://user-images.githubusercontent.com/56674380/129885299-0fe689d5-e7de-45fe-9423-34113888a78d.png)

### Admin Pages:

**Admin Dashboard**

In this page you can see information such as total users, money earned last 30 days, total courses and how many users signed up for courses for last 30 days

![image](https://user-images.githubusercontent.com/56674380/129886615-83c516b4-9645-4fb7-b637-bbbf01094426.png)

**Admin Group Chat**

In this page you can easily communicate with other admins.

![image](https://user-images.githubusercontent.com/56674380/131031010-f2234a83-9211-4aee-881a-c53f2cad5ca1.png)

