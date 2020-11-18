-- Thêm bảng User
insert into users(UserName,Email,Password) values ('Nhat','nhat@gmail.com','123')
insert into users(UserName,Email,Password) values ('Long','long@gmail.com','123')
insert into users(UserName,Email,Password) values ('Ronaldo','Ronaldo@gmail.com','123')
insert into users(UserName,Email,Password) values ('Measx','Measx@gmail.com','123')
insert into users(UserName,Email,Password) values ('Oins','Oins@gmail.com','123')
insert into users(UserName,Email,Password) values ('ALnC','AlnC@gmail.com','123')
insert into users(UserName,Email,Password) values ('Thai','Thai@gmail.com','123')
insert into users(UserName,Email,Password) values ('Tai','tai@gmail.com','123')

--Thêm bảng Articles
insert into Articles(Title,Description,Body) values ('Articles1','Description1','body1')
insert into Articles(Title,Description,Body) values ('Articles2','Description2','body2')
insert into Articles(Title,Description,Body) values ('Articles3','Description3','body3')
insert into Articles(Title,Description,Body) values ('Articles4','Description4','body4')
insert into Articles(Title,Description,Body) values ('Articles5','Description5','body5')
insert into Articles(Title,Description,Body) values ('Articles6','Description6','body6')
insert into Articles(Title,Description,Body) values ('Articles7','Description7','body7')
insert into Articles(Title,Description,Body) values ('Articles8','Description8','body8')
insert into Articles(Title,Description,Body) values ('Articles9','Description9','body9')

-- Thêm bảng userFllow
insert into Users_Following(UserID) values ('DA5FBACE-81C0-4A18-837D-0CA6A7D875D9')
insert into Users_Following(UserID) values ('B05D5E1F-D9AF-40E0-90C4-5AD36ADAF04F')
insert into Users_Following(UserID) values ('9E3E376F-C92D-47B1-B70D-7E52A736E534')
insert into Users_Following(UserID) values ('47997643-7799-4A7B-A3D2-86F23D783FC7')
insert into Users_Following(UserID) values ('835A267E-2F48-4AF1-AB9D-A8050CF62C52')
insert into Users_Following(UserID) values ('5A25E8BD-4F11-4F36-A225-B495BB32CE9D')
insert into Users_Following(UserID) values ('B98B4AAE-9189-4B7D-B9B8-B4E5CA04FDA6')
insert into Users_Following(UserID) values ('D970727F-700F-4072-9471-CF94A8E20C49')

-- Thêm bảng favotired
insert into Favotired (ArticlesID) values ('FAB0793A-F0CD-400C-BE22-17395FFEC186')
insert into Favotired (ArticlesID) values ('F4F191BA-B0F7-48F3-9DA7-470153C473EB')
insert into Favotired (ArticlesID) values ('21075466-2D2F-4DEB-B77D-7B0AC66773E7')
insert into Favotired (ArticlesID) values ('4EEE1505-C57E-444B-BB60-8D9E9AF2E646')
insert into Favotired (ArticlesID) values ('3F92ECF8-68A7-4597-8241-8EF2AA0B770E')
insert into Favotired (ArticlesID) values ('31C3833A-D419-4277-A00D-A46F82025BEE')
insert into Favotired (ArticlesID) values ('7AD3EBB5-8180-4560-8C38-D1D4FCAC7B95')
insert into Favotired (ArticlesID) values ('6331115F-9621-4464-89AF-DFFB3ABC43A2')
insert into Favotired (ArticlesID) values ('3BADFBEE-E8E0-4F13-9708-E14B1298210B')


select * from Articles 
select * from Users_Following
select * from comment
select * from Users
select * from Favotired where ArticlesID = 'FAB0793A-F0CD-400C-BE22-17395FFEC186'and UserID = '9E3E376F-C92D-47B1-B70D-7E52A736E534'

update Favotired set Favotired = 0
delete Users_Following



