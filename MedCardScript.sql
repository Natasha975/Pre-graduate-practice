CREATE TABLE [dbo].[attachment](
	[id_attachment] [int] IDENTITY(1,1) NOT NULL,
	[id_hospital] [int] NOT NULL,
	[id_patient] [int] NOT NULL,
 CONSTRAINT [PK_attachment] PRIMARY KEY CLUSTERED 
(
	[id_attachment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[disability_certificate]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[disability_certificate](
	[id_disability_certificate] [int] IDENTITY(1,1) NOT NULL,
	[id_inspection] [int] NOT NULL,
	[type_of_certificate] [nvarchar](max) NOT NULL,
	[cause_of_illness] [nvarchar](max) NOT NULL,
	[period] [int] NOT NULL,
 CONSTRAINT [PK_disability_certificate] PRIMARY KEY CLUSTERED 
(
	[id_disability_certificate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[doctor]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[doctor](
	[id_doctor] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[lastname] [nvarchar](100) NOT NULL,
	[surname] [nvarchar](100) NOT NULL,
	[id_specialization] [int] NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_doctor] PRIMARY KEY CLUSTERED 
(
	[id_doctor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hospital]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hospital](
	[id_hospital] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[region] [nvarchar](max) NULL,
	[address] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_hospital] PRIMARY KEY CLUSTERED 
(
	[id_hospital] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inspection]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inspection](
	[id_inspection] [int] IDENTITY(1,1) NOT NULL,
	[id_patient] [int] NOT NULL,
	[id_doctor] [int] NOT NULL,
	[date_inspection] [date] NOT NULL,
	[time_inspection] [time](7) NOT NULL,
	[place_of_service] [nvarchar](100) NOT NULL,
	[type_of_service_case] [nvarchar](100) NOT NULL,
	[type_of_payment] [nvarchar](225) NOT NULL,
	[purpose_of_the_service] [nvarchar](225) NOT NULL,
	[complaints] [nvarchar](max) NOT NULL,
	[medical_history] [nvarchar](max) NULL,
	[height] [int] NULL,
	[weight] [int] NULL,
	[blood_pressure_upper] [int] NULL,
	[blood_pressure_lower] [int] NULL,
	[temperature] [float] NULL,
	[heart_rate] [int] NULL,
	[respiratory_rate] [int] NULL,
	[oxygen_saturation] [int] NULL,
	[preliminary_diagnosis] [nvarchar](max) NULL,
	[the_main_diagnosis] [nvarchar](max) NULL,
	[patient_condition] [nvarchar](max) NULL,
	[suspicion_of_heat] [nvarchar](max) NULL,
	[treatment] [nvarchar](max) NOT NULL,
	[recommendations] [nvarchar](max) NULL,
 CONSTRAINT [PK_inspection] PRIMARY KEY CLUSTERED 
(
	[id_inspection] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[patient]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patient](
	[id_patient] [int] IDENTITY(1,1) NOT NULL,
	[snils] [nvarchar](14) NOT NULL,
	[lastname] [nvarchar](100) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[surname] [nvarchar](100) NULL,
	[birthday] [date] NOT NULL,
	[age] [tinyint] NOT NULL,
	[gender] [nvarchar](15) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[email] [nvarchar](225) NULL,
	[phone] [nvarchar](50) NOT NULL,
	[passport_number_and_series] [nvarchar](50) NULL,
	[place_of_work] [nvarchar](max) NULL,
	[number_med_card] [nvarchar](20) NULL,
	[date_out_med_card] [date] NULL,
	[photo] [varbinary](max) NULL,
	[name_insurance_company] [nvarchar](max) NOT NULL,
	[number_policy_OMS] [nvarchar](16) NOT NULL,
	[date_and_insur_policy] [date] NULL,
 CONSTRAINT [PK_patient] PRIMARY KEY CLUSTERED 
(
	[id_patient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[period]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[period](
	[id_period] [int] IDENTITY(1,1) NOT NULL,
	[id_disability_certificate] [int] NOT NULL,
	[date_start] [date] NOT NULL,
	[date_end] [date] NOT NULL,
 CONSTRAINT [PK_period] PRIMARY KEY CLUSTERED 
(
	[id_period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[prescription_form_107у]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[prescription_form_107у](
	[number] [int] IDENTITY(1,1) NOT NULL,
	[id_inspection] [int] NOT NULL,
	[date] [date] NOT NULL,
	[name_of_the_drug] [nvarchar](max) NULL,
	[dosage] [nvarchar](max) NULL,
	[method_of_administration] [nvarchar](max) NULL,
	[method_of_administration_details] [nvarchar](max) NULL,
	[dosage_regimen] [nvarchar](max) NULL,
	[dosage_regimen_optional] [nvarchar](max) NULL,
	[duration_of_treatment_number] [nvarchar](max) NULL,
	[duration_of_treatment_duration] [nvarchar](max) NULL,
	[duration_of_treatment_comments] [nvarchar](max) NULL,
	[justification_of_appointment] [nvarchar](max) NULL,
 CONSTRAINT [PK_prescription_form_107у] PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[referral]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[referral](
	[id_referral] [int] IDENTITY(1,1) NOT NULL,
	[id_inspection] [int] NOT NULL,
	[type_of_direction] [nvarchar](max) NULL,
	[date_of_creation] [date] NULL,
	[payment_source] [nvarchar](max) NULL,
	[service] [nvarchar](max) NULL,
	[organization] [nvarchar](max) NULL,
	[doctor] [nvarchar](max) NULL,
	[date_of_admission] [date] NULL,
	[justification] [nvarchar](max) NULL,
 CONSTRAINT [PK_referral] PRIMARY KEY CLUSTERED 
(
	[id_referral] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[representative_patient]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[representative_patient](
	[number_representative] [int] NOT NULL,
	[name_representative] [nvarchar](11) NOT NULL,
	[lastname] [nvarchar](100) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[surname] [nvarchar](100) NOT NULL,
	[document] [date] NOT NULL,
	[series] [nvarchar](3) NOT NULL,
	[number] [int] NOT NULL,
	[issued_by_whom] [nvarchar](max) NOT NULL,
	[release_datet_the_document] [nvarchar](max) NOT NULL,
	[phone] [nvarchar](50) NOT NULL,
	[id_patient] [int] NOT NULL,
 CONSTRAINT [PK_representative_patient] PRIMARY KEY CLUSTERED 
(
	[number_representative] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[specialization]    Script Date: 19.05.2025 11:06:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[specialization](
	[id_specialization] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_specialization] PRIMARY KEY CLUSTERED 
(
	[id_specialization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[attachment] ON 
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (1, 8, 1)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (2, 25, 2)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (3, 14, 3)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (4, 18, 4)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (5, 9, 5)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (6, 24, 6)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (7, 29, 7)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (8, 17, 8)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (9, 2, 9)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (10, 17, 10)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (11, 7, 11)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (12, 19, 12)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (13, 15, 13)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (14, 7, 14)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (15, 13, 15)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (16, 1, 16)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (17, 9, 17)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (18, 2, 18)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (19, 25, 19)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (20, 6, 20)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (21, 16, 21)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (22, 23, 22)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (23, 2, 23)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (24, 27, 24)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (25, 9, 25)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (26, 22, 26)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (27, 25, 27)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (28, 25, 28)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (29, 23, 29)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (30, 22, 30)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (31, 3, 31)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (32, 16, 32)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (33, 7, 33)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (34, 21, 34)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (35, 26, 35)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (36, 16, 36)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (37, 26, 37)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (38, 5, 38)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (39, 21, 39)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (40, 26, 40)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (41, 19, 41)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (42, 1, 42)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (43, 12, 43)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (44, 4, 44)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (45, 16, 45)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (46, 22, 46)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (47, 12, 47)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (48, 16, 48)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (49, 10, 49)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (50, 16, 50)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (51, 29, 51)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (52, 20, 52)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (53, 29, 53)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (54, 9, 54)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (55, 10, 55)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (56, 16, 56)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (57, 3, 57)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (58, 27, 58)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (59, 27, 59)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (60, 19, 60)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (61, 19, 61)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (62, 28, 62)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (63, 3, 63)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (64, 26, 64)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (65, 6, 65)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (66, 27, 66)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (67, 26, 67)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (68, 4, 68)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (69, 20, 69)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (70, 17, 70)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (71, 29, 71)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (72, 5, 72)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (73, 1, 73)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (74, 4, 74)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (75, 23, 75)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (76, 21, 76)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (77, 6, 77)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (78, 6, 78)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (79, 4, 79)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (80, 3, 80)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (81, 10, 81)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (82, 25, 82)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (83, 27, 83)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (84, 15, 84)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (85, 9, 85)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (86, 26, 86)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (87, 8, 87)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (88, 12, 88)
GO
INSERT [dbo].[attachment] ([id_attachment], [id_hospital], [id_patient]) VALUES (89, 19, 89)
GO
SET IDENTITY_INSERT [dbo].[attachment] OFF
GO
SET IDENTITY_INSERT [dbo].[doctor] ON 
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (1, N'Иванов', N'Алексей', N'Петрович', 1, N'ivanov', N'Ivanov2024')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (2, N'Петрова', N'Мария', N'Сергеевна', 2, N'petrova', N'Petrova789')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (3, N'Сидоров', N'Дмитрий', N'Игоревич', 3, N'sidorov', N'SidorovMed#456')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (4, N'Кузнецова', N'Елена', N'Владимировна', 4, N'kuznetsova', N'Kuzn3ts0va!')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (5, N'Смирнов', N'Андрей', N'Александрович', 5, N'smirnov', N'Sm1rn0vAndr@y')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (6, N'Сидорова', N'Анна', N'Алексеевна', 3, N'sidorova.a', N'T9^mX6!yU4#qW')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (7, N'Васильева', N'Ольга', N'Петровна', 6, N'vasileva.o', N'X6%nJ3@tG7^hD')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (8, N'Попов', N'Сергей', N'Сергеевич', 7, N'popov.s', N'Q9$fK5!rL2#mP')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (9, N'Михайлова', N'Елена', N'Андреевна', 8, N'mihailova.e', N'W3@yH8%jD4&vS')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (10, N'Федоров', N'Алексей', N'Алексеевич', 9, N'fedorov.a', N'B7^tN2!mK5#pZ')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (11, N'Морозова', N'Татьяна', N'Викторовна', 10, N'morozova.t', N'S4$dF9@gH2%jL')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (12, N'Волков', N'Андрей', N'Андреевич', 11, N'volkov.a', N'C8!kP3#mZ6$qX')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (13, N'Алексеева', N'Юлия', N'Юрьевна', 12, N'alekseeva.y', N'N5%vJ9@bM2^hL')
GO
INSERT [dbo].[doctor] ([id_doctor], [name], [lastname], [surname], [id_specialization], [login], [password]) VALUES (14, N'Лебедев', N'Артем', N'Артемович', 13, N'lebedev.a', N'F7$gK4!rT1#pY')
GO
SET IDENTITY_INSERT [dbo].[doctor] OFF
GO
SET IDENTITY_INSERT [dbo].[hospital] ON 
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (1, N'Городская клиническая больница №1', N'Москва', N'ул. Ленина, 10')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (2, N'Центральная районная больница', N'Московская область', N'ул. Центральная, 5')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (3, N'Детская городская больница', N'Санкт-Петербург', N'Невский пр-т, 45')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (4, N'Областная клиническая больница', N'Ленинградская область', N'ул. Медицинская, 12')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (5, N'Городская больница скорой помощи', N'Новосибирск', N'ул. Красный проспект, 20')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (6, N'Клинический центр им. Семашко', N'Москва', N'ул. Щепкина, 61/2')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (7, N'Краевая больница №2', N'Краснодарский край', N'ул. Красных Партизан, 6')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (8, N'Городская больница №3', N'Екатеринбург', N'ул. Бажова, 124')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (9, N'Областная детская клиническая больница', N'Свердловская область', N'ул. Репина, 1')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (10, N'Центральная городская больница', N'Казань', N'ул. Карла Маркса, 15')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (11, N'Республиканская больница', N'Татарстан', N'ул. Оренбургский тракт, 138')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (12, N'Городская клиническая больница №4', N'Челябинск', N'ул. Воровского, 16')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (13, N'Клинический госпиталь', N'Москва', N'Госпитальная пл., 2')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (14, N'Городская больница №5', N'Омск', N'ул. Лермонтова, 40')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (15, N'Областная больница', N'Ростовская область', N'ул. 1-я Линия, 3')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (16, N'Детская республиканская больница', N'Башкортостан', N'ул. Ст. Кувыкина, 96')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (17, N'Городская клиническая больница №7', N'Самара', N'ул. Полевая, 80')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (18, N'Краевая клиническая больница', N'Пермский край', N'ул. Пушкина, 85')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (19, N'Городская больница скорой медицинской помощи', N'Воронеж', N'ул. 45 Стрелковой Дивизии, 64')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (20, N'Областной клинический центр', N'Волгоградская область', N'ул. Ангарская, 13')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (21, N'Центральная районная больница №2', N'Красноярский край', N'ул. Академика Киренского, 2')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (22, N'Городская больница №9', N'Уфа', N'ул. Ст. Кувыкина, 80')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (23, N'Клиническая инфекционная больница', N'Москва', N'ул. Вольская, 7')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (24, N'Областная больница №3', N'Иркутская область', N'ул. Байкальская, 120')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (25, N'Городская детская клиническая больница', N'Краснодар', N'ул. Московская, 65')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (26, N'Центральная городская клиническая больница', N'Нижний Новгород', N'ул. Родионова, 190')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (27, N'Городская больница №12', N'Кемерово', N'пр. Октябрьский, 22')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (28, N'Краевая детская клиническая больница', N'Ставропольский край', N'ул. Семашко, 3')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (29, N'Городская клиническая больница им. Боткина', N'Москва', N'2-й Боткинский пр-д, 5')
GO
INSERT [dbo].[hospital] ([id_hospital], [name], [region], [address]) VALUES (30, N'Областная клиническая больница №4', N'Тюменская область', N'ул. Котовского, 55')
GO
SET IDENTITY_INSERT [dbo].[hospital] OFF
GO
SET IDENTITY_INSERT [dbo].[inspection] ON 
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (1, 1, 1, CAST(N'2025-04-11' AS Date), CAST(N'09:30:00' AS Time), N'Кабинет 101', N'Первичный', N'ОМС', N'Диагностика', N'Головная боль, слабость', N'Гипертония 2 степени', 178, 85, 180, 110, 36.6, 88, 18, 97, N'Гипертонический криз', N'Гипертоническая болезнь 2 ст.', N'Средней тяжести', N'Нет', N'Каптоприл 25 мг 2 раза в день', N'Контроль АД')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (2, 1, 3, CAST(N'2025-04-13' AS Date), CAST(N'11:30:00' AS Time), N'Кабинет 305', N'Консультация', N'ОМС', N'Кардиологическая консультация', N'Периодические боли в груди', N'Гипертония, ИБС', 178, 85, 150, 95, 36.8, 82, 16, 98, N'Стенокардия', N'ИБС: стенокардия напряжения I ФК', N'Удовлетворительное', N'Нет', N'Аспирин 75 мг/сут', N'Холтеровское мониторирование')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (3, 2, 2, CAST(N'2025-04-11' AS Date), CAST(N'10:15:00' AS Time), N'Кабинет 205', N'Повторный', N'ОМС', N'Контроль лечения', N'Боли в животе', N'Хронический гастрит', 165, 58, 120, 80, 36.6, 76, 16, 99, N'Обострение гастрита', N'Хронический гастрит', N'Удовлетворительное', N'Нет', N'Омепразол 20 мг 2 раза в день', N'Диета №1')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (4, 3, 3, CAST(N'2025-04-11' AS Date), CAST(N'11:00:00' AS Time), N'Кабинет 310', N'Первичный', N'ОМС', N'Диагностика', N'Боли в сердце', N'ИБС', 182, 92, 140, 90, 36.7, 78, 17, 96, N'Стенокардия напряжения', N'ИБС: стенокардия напряжения II ФК', N'Удовлетворительное', N'Нет', N'Нитроглицерин по требованию', N'Ограничение физ.нагрузок')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (5, 4, 4, CAST(N'2025-04-11' AS Date), CAST(N'14:30:00' AS Time), N'Кабинет 402', N'Первичный', N'ОМС', N'Консультация', N'Головокружение', N'ВСД', 170, 62, 110, 70, 36.5, 85, 18, 98, N'Вертебробазилярная недостаточность', N'ВСД', N'Удовлетворительное', N'Нет', N'Бетасерк 16 мг 3 раза в день', N'Контроль невролога')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (6, 5, 5, CAST(N'2025-04-11' AS Date), CAST(N'15:45:00' AS Time), N'Кабинет 512', N'Повторный', N'ОМС', N'Контроль лечения', N'Снижение зрения', N'Миопия средней степени', 185, 78, 125, 80, 36.7, 72, 15, 99, N'Прогрессирующая миопия', N'Миопия средней степени', N'Удовлетворительное', N'Нет', N'Гимнастика для глаз', N'Контроль через 6 месяцев')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (7, 1, 1, CAST(N'2025-04-15' AS Date), CAST(N'23:05:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'2 - консультативная', N'Головокружение', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 180, 70, 120, 70, 0, 70, 21, 20, NULL, NULL, N'Удовлетворительное', N'Нет', N'', N'Контроль АД')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (8, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'10:25:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'Насморк', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Астма', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (9, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'01:11:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Пневмония без уточнения возбудителя', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (10, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'01:21:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Другие гастроэнтериты и колиты инфекционного происхождения', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (11, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'20:45:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Другие гастроэнтериты и колиты инфекционного происхождения', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (12, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'21:15:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'Одышка', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Пневмония без уточнения возбудителя', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (13, 1, 1, CAST(N'2025-05-12' AS Date), CAST(N'21:23:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'Головные боли', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Острые инфекции верхних дыхательных путей', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (14, 1, 1, CAST(N'2025-05-13' AS Date), CAST(N'18:38:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'Головные боли, Боли в грудной клетке', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Хроническая ишемическая болезнь сердца', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (15, 1, 1, CAST(N'2025-05-15' AS Date), CAST(N'20:11:00' AS Time), N'1 - Поликлиника', N'1 - Первичный', N'ОМС', N'1 - Лечебно-диагностическая', N'Головокружение', N'Перенесенные заболевания: Хранические заболевания отрицает.Эпидемиологический анамнез: Туберкулез, непатиты, ВИЧ, венерические и другие инфекционные заболевания отрицает.Аллергический анамнез: спокойный.Наследственность: не отягощена.', 160, 60, 120, 80, 0, 73, 20, 20, NULL, N'Дорсалгия', N'Удовлетворительное', N'Нет', N'', N'')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (17, 2, 8, CAST(N'2025-04-02' AS Date), CAST(N'10:15:00' AS Time), N'Поликлиника №2', N'Повторный осмотр', N'ДМС', N'Контроль лечения', N'Кашель, температура', N'ОРВИ, бронхит в анамнезе', 165, 58, 120, 80, 37.8, 85, 20, 96, N'ОРВИ', N'Острый бронхит', N'Средней тяжести', N'Нет', N'Амоксиклав 625 мг 3 раза в день, обильное питье', N'Повторный осмотр через 3 дня')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (18, 3, 12, CAST(N'2025-04-03' AS Date), CAST(N'11:00:00' AS Time), N'Травмпункт', N'Неотложная помощь', N'ОМС', N'Травма конечности', N'Боль в правом колене после падения', N'Перелом лодыжки в 2020', 182, 90, 130, 85, 36.9, 72, 18, 99, N'Ушиб коленного сустава', N'Ушиб правого коленного сустава', N'Удовлетворительное', NULL, N'Холод местно, диклофенак гель 3 раза в день', N'Ограничить нагрузку на ногу 3 дня')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (19, 4, 3, CAST(N'2025-04-04' AS Date), CAST(N'14:20:00' AS Time), N'Женская консультация', N'Плановый осмотр', N'ОМС', N'Профилактический осмотр', N'Жалоб нет', N'Беременность 12 недель', 170, 65, 110, 70, 36.8, 80, 16, 98, N'Беременность', N'Беременность 12 недель', N'Удовлетворительное', NULL, N'Фолиевая кислота 400 мкг/сут', N'Повторный прием через 4 недели')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (20, 5, 7, CAST(N'2025-04-07' AS Date), CAST(N'15:45:00' AS Time), N'Поликлиника №3', N'Первичный осмотр', N'ДМС', N'Боль в спине', N'Острая боль в пояснице', N'Остеохондроз', 185, 95, 135, 85, 37, 76, 17, 97, N'Остеохондроз', N'Обострение остеохондроза поясничного отдела', N'Удовлетворительное', NULL, N'Диклофенак 75 мг/сут в/м 3 дня, мидокалм 150 мг/сут', N'Ограничить физические нагрузки')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (21, 6, 9, CAST(N'2025-04-08' AS Date), CAST(N'08:30:00' AS Time), N'Поликлиника №1', N'Повторный осмотр', N'ОМС', N'Контроль сахара крови', N'Жажда, сухость во рту', N'Сахарный диабет 2 типа', 168, 70, 125, 80, 36.7, 82, 16, 97, N'Декомпенсация СД', N'Сахарный диабет 2 типа, субкомпенсация', N'Удовлетворительное', NULL, N'Коррекция дозы метформина до 1000 мг 2 раза в день', N'Контроль глюкозы крови 3 раза в день')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (22, 7, 13, CAST(N'2025-04-09' AS Date), CAST(N'12:00:00' AS Time), N'Кардиоцентр', N'Консультация специалиста', N'ДМС', N'Боли в сердце', N'Давящие боли за грудиной при нагрузке', N'ИБС, стенокардия напряжения', 180, 88, 150, 95, 36.6, 90, 18, 96, N'Стенокардия', N'ИБС, стенокардия напряжения II ФК', N'Средней тяжести', NULL, N'Нитроглицерин спрей, бисопролол 5 мг/сут', N'Ограничить физические нагрузки')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (23, 8, 4, CAST(N'2025-04-10' AS Date), CAST(N'09:45:00' AS Time), N'Поликлиника №2', N'Профосмотр', N'ОМС', N'Ежегодный профосмотр', N'Жалоб нет', NULL, 172, 62, 115, 75, 36.5, 78, 16, 98, N'Здоров', N'Практически здоров', N'Удовлетворительное', NULL, N'Не требуется', N'Повторить профосмотр через год')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (24, 9, 11, CAST(N'2025-04-11' AS Date), CAST(N'16:30:00' AS Time), N'Травмпункт', N'Неотложная помощь', N'ОМС', N'Травма руки', N'Боль в левом запястье после удара', NULL, 190, 92, 130, 85, 37.1, 85, 18, 97, N'Ушиб запястья', N'Ушиб левого запястья', N'Удовлетворительное', NULL, N'Фиксация эластичным бинтом 3 дня, нурофен 400 мг при боли', N'Контроль через 2 дня')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (25, 10, 6, CAST(N'2025-04-14' AS Date), CAST(N'10:00:00' AS Time), N'Поликлиника №3', N'Первичный осмотр', N'ДМС', N'Головные боли', N'Периодические головные боли', N'Мигрень', 175, 60, 120, 80, 36.7, 80, 17, 98, N'Головная боль напряжения', N'Мигрень без ауры', N'Удовлетворительное', NULL, N'Ибупрофен 400 мг при приступе, массаж воротниковой зоны', N'Контроль через 2 недели')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (26, 11, 10, CAST(N'2025-04-15' AS Date), CAST(N'11:15:00' AS Time), N'Поликлиника №1', N'Повторный осмотр', N'ОМС', N'Контроль давления', N'Головокружение', N'Гипертония 1 ст.', 183, 87, 145, 90, 36.8, 82, 18, 97, N'Гипертонический криз', N'Гипертоническая болезнь I стадии', N'Средней тяжести', NULL, N'Эналаприл 5 мг 2 раза в день', N'Контроль АД 2 раза в день')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (27, 12, 14, CAST(N'2025-04-16' AS Date), CAST(N'14:00:00' AS Time), N'Гинекология', N'Плановый осмотр', N'ДМС', N'Профилактический осмотр', N'Жалоб нет', NULL, 167, 59, 110, 70, 36.5, 78, 16, 98, N'Здорова', N'Практически здорова', N'Удовлетворительное', NULL, N'Не требуется', N'Повторный осмотр через год')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (28, 13, 2, CAST(N'2025-04-17' AS Date), CAST(N'08:45:00' AS Time), N'Поликлиника №2', N'Первичный осмотр', N'ОМС', N'Боль в горле', N'Боль при глотании, температура', NULL, 188, 93, 125, 80, 38.2, 90, 20, 96, N'Острый фарингит', N'Острый фарингит', N'Средней тяжести', N'Нет', N'Амоксициллин 500 мг 3 раза в день 7 дней', N'Обильное теплое питье')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (29, 14, 13, CAST(N'2025-04-18' AS Date), CAST(N'13:30:00' AS Time), N'Поликлиника №3', N'Повторный осмотр', N'ДМС', N'Контроль лечения', N'Улучшение состояния', N'Аллергический ринит', 173, 61, 115, 75, 36.6, 80, 17, 98, N'Ремиссия', N'Аллергический ринит, ремиссия', N'Удовлетворительное', NULL, N'Продолжить прием цетиризина 10 мг/сут', N'Повторить прием через месяц')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (30, 15, 1, CAST(N'2025-04-21' AS Date), CAST(N'15:00:00' AS Time), N'Травмпункт', N'Неотложная помощь', N'ОМС', N'Травма ноги', N'Боль в голеностопе после подворота ноги', NULL, 184, 89, 130, 85, 37, 84, 18, 97, N'Растяжение связок', N'Растяжение связок правого голеностопного сустава', N'Удовлетворительное', NULL, N'Фиксация эластичным бинтом, нурофен 400 мг при боли', N'Ограничить нагрузку 5 дней')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (31, 16, 10, CAST(N'2025-04-22' AS Date), CAST(N'09:15:00' AS Time), N'Поликлиника №1', N'Профосмотр', N'ОМС', N'Ежегодный профосмотр', N'Жалоб нет', N'Гастрит', 171, 63, 120, 80, 36.5, 78, 16, 98, N'Здорова', N'Хронический гастрит вне обострения', N'Удовлетворительное', NULL, N'Диета №1', N'Повторить профосмотр через год')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (32, 17, 9, CAST(N'2025-04-23' AS Date), CAST(N'11:45:00' AS Time), N'Поликлиника №2', N'Первичный осмотр', N'ДМС', N'Боль в животе', N'Тошнота, боль в эпигастрии', NULL, 186, 91, 125, 80, 37.2, 85, 18, 97, N'Острый гастрит', N'Острый гастрит', N'Средней тяжести', NULL, N'Омепразол 20 мг 2 раза в день, диета', N'Контроль через 3 дня')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (33, 18, 7, CAST(N'2025-04-24' AS Date), CAST(N'14:15:00' AS Time), N'Женская консультация', N'Плановый осмотр', N'ОМС', N'Профилактический осмотр', N'Жалоб нет', NULL, 169, 64, 115, 75, 36.6, 80, 17, 98, N'Здорова', N'Практически здорова', N'Удовлетворительное', NULL, N'Не требуется', N'Повторный осмотр через год')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (34, 19, 10, CAST(N'2025-04-25' AS Date), CAST(N'10:30:00' AS Time), N'Поликлиника №3', N'Повторный осмотр', N'ДМС', N'Контроль давления', N'Головные боли уменьшились', N'Гипертония', 187, 94, 135, 85, 36.7, 82, 18, 97, N'Гипертония', N'Гипертоническая болезнь I стадии', N'Удовлетворительное', NULL, N'Продолжить прием эналаприла 5 мг/сут', N'Контроль АД ежедневно')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (35, 20, 5, CAST(N'2025-04-28' AS Date), CAST(N'12:00:00' AS Time), N'Поликлиника №1', N'Первичный осмотр', N'ОМС', N'Кашель', N'Сухой кашель 3 дня', NULL, 170, 65, 120, 80, 37.5, 85, 19, 96, N'Острый трахеит', N'Острый трахеит', N'Удовлетворительное', N'Нет', N'Амброксол 30 мг 3 раза в день, обильное питье', N'Повторный осмотр при ухудшении')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (36, 1, 5, CAST(N'2025-05-05' AS Date), CAST(N'09:30:00' AS Time), N'Поликлиника №1', N'Повторный осмотр', N'ОМС', N'Контроль АД', N'Состояние улучшилось', N'Гипертония, 2 стадия', 178, 85, 130, 85, 36.6, 75, 16, 98, N'Гипертоническая болезнь', N'Гипертоническая болезнь II стадии', N'Удовлетворительное', NULL, N'Продолжить прием каптоприла', N'Контроль АД 2 раза в день')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (37, 2, 8, CAST(N'2025-05-06' AS Date), CAST(N'10:15:00' AS Time), N'Поликлиника №2', N'Повторный осмотр', N'ДМС', N'Контроль лечения', N'Кашель уменьшился', N'ОРВИ, бронхит в анамнезе', 165, 58, 115, 75, 36.8, 80, 18, 97, N'Выздоровление', N'Острый бронхит, выздоровление', N'Удовлетворительное', N'Нет', N'Отменить антибиотики, продолжать питьевой режим', N'Повторный осмотр при необходимости')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (38, 3, 12, CAST(N'2025-05-07' AS Date), CAST(N'11:00:00' AS Time), N'Травмпункт', N'Повторный осмотр', N'ОМС', N'Контроль травмы', N'Боль уменьшилась', N'Ушиб колена', 182, 90, 125, 80, 36.7, 70, 16, 98, N'Выздоровление', N'Ушиб правого коленного сустава, выздоровление', N'Удовлетворительное', NULL, N'Отменить лечение', N'Возобновить обычную нагрузку')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (39, 4, 3, CAST(N'2025-05-08' AS Date), CAST(N'14:20:00' AS Time), N'Женская консультация', N'Плановый осмотр', N'ОМС', N'Контроль беременности', N'Жалоб нет', N'Беременность 16 недель', 170, 67, 110, 70, 36.7, 82, 16, 98, N'Беременность', N'Беременность 16 недель', N'Удовлетворительное', NULL, N'Продолжить прием фолиевой кислоты', N'Повторный прием через 4 недели')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (40, 5, 7, CAST(N'2025-05-12' AS Date), CAST(N'15:45:00' AS Time), N'Поликлиника №3', N'Повторный осмотр', N'ДМС', N'Контроль состояния', N'Боль в спине уменьшилась', N'Остеохондроз', 185, 95, 130, 80, 36.6, 74, 16, 98, N'Улучшение', N'Остеохондроз поясничного отдела, улучшение', N'Удовлетворительное', NULL, N'Продолжить мидокалм 150 мг/сут 5 дней', N'ЛФК через неделю')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (41, 6, 9, CAST(N'2025-05-13' AS Date), CAST(N'08:30:00' AS Time), N'Поликлиника №1', N'Повторный осмотр', N'ОМС', N'Контроль сахара крови', N'Состояние стабильное', N'Сахарный диабет 2 типа', 168, 70, 120, 80, 36.6, 80, 16, 97, N'Компенсация', N'Сахарный диабет 2 типа, компенсация', N'Удовлетворительное', NULL, N'Продолжить текущее лечение', N'Контроль глюкозы крови 2 раза в день')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (42, 7, 13, CAST(N'2025-05-14' AS Date), CAST(N'12:00:00' AS Time), N'Кардиоцентр', N'Повторный осмотр', N'ДМС', N'Контроль ИБС', N'Боли реже', N'ИБС, стенокардия напряжения', 180, 88, 140, 90, 36.6, 85, 17, 97, N'Стабильное', N'ИБС, стенокардия напряжения II ФК', N'Удовлетворительное', NULL, N'Продолжить текущее лечение', N'Повторный осмотр через месяц')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (43, 8, 4, CAST(N'2025-05-15' AS Date), CAST(N'09:45:00' AS Time), N'Поликлиника №2', N'Консультация', N'ОМС', N'Профилактический осмотр', N'Жалоб нет', NULL, 172, 62, 115, 75, 36.5, 76, 16, 98, N'Здоров', N'Практически здоров', N'Удовлетворительное', NULL, N'Не требуется', N'Повторить осмотр через год')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (44, 9, 11, CAST(N'2025-05-16' AS Date), CAST(N'16:30:00' AS Time), N'Травмпункт', N'Повторный осмотр', N'ОМС', N'Контроль травмы', N'Боль прошла', N'Ушиб запястья', 190, 92, 125, 80, 36.6, 72, 16, 98, N'Выздоровление', N'Ушиб левого запястья, выздоровление', N'Удовлетворительное', NULL, N'Отменить лечение', N'Возобновить обычную нагрузку')
GO
INSERT [dbo].[inspection] ([id_inspection], [id_patient], [id_doctor], [date_inspection], [time_inspection], [place_of_service], [type_of_service_case], [type_of_payment], [purpose_of_the_service], [complaints], [medical_history], [height], [weight], [blood_pressure_upper], [blood_pressure_lower], [temperature], [heart_rate], [respiratory_rate], [oxygen_saturation], [preliminary_diagnosis], [the_main_diagnosis], [patient_condition], [suspicion_of_heat], [treatment], [recommendations]) VALUES (45, 10, 6, CAST(N'2025-05-19' AS Date), CAST(N'10:00:00' AS Time), N'Поликлиника №3', N'Повторный осмотр', N'ДМС', N'Контроль мигрени', N'Головные боли реже', N'Мигрень', 175, 60, 120, 80, 36.6, 78, 16, 98, N'Улучшение', N'Мигрень без ауры, улучшение', N'Удовлетворительное', NULL, N'Продолжить текущее лечение', N'Повторный осмотр через месяц')
GO
SET IDENTITY_INSERT [dbo].[inspection] OFF
GO
SET IDENTITY_INSERT [dbo].[patient] ON 
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (1, N'123-456-789 01', N'Иванов', N'Иван', N'Иванович', CAST(N'1980-05-15' AS Date), 45, N'Мужской', N'г. Москва, ул. Ленина, д. 10, кв. 5', N'ivanov@mail.ru', N'+79161234567', N'4510 123456', N'ООО "Технологии Будущего"', N'MC-00001', CAST(N'2025-01-15' AS Date), NULL, N'СОГАЗ-Мед', N'1234567890123456', CAST(N'2025-01-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (2, N'234-567-890 12', N'Петрова', N'Мария', N'Сергеевна', CAST(N'1992-08-22' AS Date), 32, N'Женский', N'г. Санкт-Петербург, Невский пр-т, д. 50, кв. 12', N'petrova@yandex.ru', N'+79172345678', N'4611 234567', N'АО "Банк Столичный"', N'MC-00002', CAST(N'2025-02-10' AS Date), NULL, N'Росгосстрах', N'2345678901234567', CAST(N'2025-02-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (3, N'345-678-901 23', N'Сидоров', N'Алексей', N'Дмитриевич', CAST(N'1975-11-30' AS Date), 49, N'Мужской', N'г. Екатеринбург, ул. Мира, д. 25, кв. 7', N'sidorov@gmail.com', N'+79183456789', N'4712 345678', N'ЗАО "Промышленные Решения"', N'MC-00003', CAST(N'2025-03-05' AS Date), NULL, N'АльфаСтрахование', N'3456789012345678', CAST(N'2025-03-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (4, N'456-789-012 34', N'Кузнецова', N'Елена', N'Викторовна', CAST(N'1988-03-10' AS Date), 37, N'Женский', N'г. Казань, ул. Баумана, д. 30, кв. 15', N'kuznetsova@mail.ru', N'+79194567890', N'4813 456789', N'ООО "Медицинские Технологии"', N'MC-00004', CAST(N'2025-04-20' AS Date), NULL, N'Ингосстрах', N'4567890123456789', CAST(N'2025-04-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (5, N'567-890-123 45', N'Николаев', N'Дмитрий', N'Алексеевич', CAST(N'1995-07-25' AS Date), 29, N'Мужской', N'г. Новосибирск, ул. Кирова, д. 15, кв. 22', N'nikolaev@yandex.ru', N'+79205678901', N'4914 567890', N'ПАО "НефтеГаз"', N'MC-00005', CAST(N'2025-05-12' AS Date), NULL, N'РЕСО-Мед', N'5678901234567890', CAST(N'2025-05-17' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (6, N'678-901-234 56', N'Смирнова', N'Ольга', N'Игоревна', CAST(N'1983-12-05' AS Date), 41, N'Женский', N'г. Нижний Новгород, ул. Горького, д. 12, кв. 8', N'smirnova@gmail.com', N'+79316789012', N'5015 678901', N'ООО "Торговый Дом"', N'MC-00006', CAST(N'2025-06-08' AS Date), NULL, N'Капитал Медицинское Страхование', N'6789012345678901', CAST(N'2025-06-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (7, N'789-012-345 67', N'Васильев', N'Андрей', N'Сергеевич', CAST(N'1978-09-18' AS Date), 46, N'Мужской', N'г. Самара, ул. Куйбышева, д. 45, кв. 31', N'vasilev@mail.ru', N'+79427890123', N'5116 789012', N'АО "Строительные Технологии"', N'MC-00007', CAST(N'2025-07-19' AS Date), NULL, N'МАКС-М', N'7890123456789012', CAST(N'2025-07-24' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (8, N'890-123-456 78', N'Попова', N'Анна', N'Александровна', CAST(N'1990-04-30' AS Date), 35, N'Женский', N'г. Омск, ул. Ленина, д. 20, кв. 14', N'popova@yandex.ru', N'+79538901234', N'5217 890123', N'ООО "Фармацевтика"', N'MC-00008', CAST(N'2025-08-22' AS Date), NULL, N'Ренессанс Здоровье', N'8901234567890123', CAST(N'2025-08-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (9, N'901-234-567 89', N'Лебедев', N'Михаил', N'Олегович', CAST(N'1985-06-12' AS Date), 39, N'Мужской', N'г. Челябинск, ул. Свободы, д. 8, кв. 9', N'lebedev@gmail.com', N'+79649012345', N'5318 901234', N'ПАО "Металлургический Завод"', N'MC-00009', CAST(N'2025-09-11' AS Date), NULL, N'Согаз-Мед', N'9012345678901234', CAST(N'2025-09-16' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (10, N'012-345-678 90', N'Козлова', N'Екатерина', N'Валерьевна', CAST(N'1993-02-28' AS Date), 32, N'Женский', N'г. Ростов-на-Дону, ул. Большая Садовая, д. 10, кв. 3', N'kozlova@mail.ru', N'+79750123456', N'5419 012345', N'ООО "Бухгалтерские Услуги"', N'MC-00010', CAST(N'2025-10-05' AS Date), NULL, N'АльфаСтрахование', N'0123456789012345', CAST(N'2025-10-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (11, N'123-456-789 02', N'Новиков', N'Павел', N'Игоревич', CAST(N'1982-07-14' AS Date), 42, N'Мужской', N'г. Уфа, ул. Ленина, д. 25, кв. 17', N'novikov@yandex.ru', N'+79861234567', N'5520 123456', N'АО "Телекоммуникации"', N'MC-00011', CAST(N'2025-11-18' AS Date), NULL, N'Росгосстрах', N'1234567890123456', CAST(N'2025-11-23' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (12, N'234-567-890 13', N'Морозова', N'Наталья', N'Сергеевна', CAST(N'1987-10-22' AS Date), 37, N'Женский', N'г. Красноярск, ул. Карла Маркса, д. 15, кв. 6', N'morozova@gmail.com', N'+79972345678', N'5621 234567', N'ООО "Медицинский Центр"', N'MC-00012', CAST(N'2025-12-07' AS Date), NULL, N'СОГАЗ-Мед', N'2345678901234567', CAST(N'2025-12-12' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (13, N'345-678-901 24', N'Волков', N'Денис', N'Андреевич', CAST(N'1979-03-08' AS Date), 46, N'Мужской', N'г. Пермь, ул. Ленина, д. 30, кв. 11', N'volkov@mail.ru', N'+70083456789', N'5722 345678', N'ЗАО "Промышленные Технологии"', N'MC-00013', CAST(N'2026-01-15' AS Date), NULL, N'Ингосстрах', N'3456789012345678', CAST(N'2026-01-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (14, N'456-789-012 35', N'Алексеева', N'Виктория', N'Олеговна', CAST(N'1991-05-19' AS Date), 34, N'Женский', N'г. Воронеж, ул. Плехановская, д. 12, кв. 9', N'alekseeva@yandex.ru', N'+70194567890', N'5823 456789', N'ООО "Фармацевтическая Компания"', N'MC-00014', CAST(N'2026-02-22' AS Date), NULL, N'РЕСО-Мед', N'4567890123456789', CAST(N'2026-02-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (15, N'567-890-123 46', N'Егоров', N'Артем', N'Викторович', CAST(N'1984-08-25' AS Date), 40, N'Мужской', N'г. Волгоград, ул. Советская, д. 10, кв. 4', N'egorov@gmail.com', N'+70205678901', N'5924 567890', N'ПАО "Нефтяная Компания"', N'MC-00015', CAST(N'2026-03-10' AS Date), NULL, N'Капитал Медицинское Страхование', N'5678901234567890', CAST(N'2026-03-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (16, N'678-901-234 57', N'Орлова', N'Светлана', N'Дмитриевна', CAST(N'1977-11-30' AS Date), 47, N'Женский', N'г. Краснодар, ул. Красная, д. 5, кв. 7', N'orlova@mail.ru', N'+70316789012', N'6025 678901', N'ООО "Торговая Сеть"', N'MC-00016', CAST(N'2026-04-18' AS Date), NULL, N'МАКС-М', N'6789012345678901', CAST(N'2026-04-23' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (17, N'789-012-345 68', N'Семенов', N'Игорь', N'Анатольевич', CAST(N'1989-01-15' AS Date), 36, N'Мужской', N'г. Саратов, ул. Московская, д. 20, кв. 12', N'semenov@yandex.ru', N'+70427890123', N'6126 789012', N'АО "Строительные Материалы"', N'MC-00017', CAST(N'2026-05-05' AS Date), NULL, N'Ренессанс Здоровье', N'7890123456789012', CAST(N'2026-05-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (18, N'890-123-456 79', N'Антонова', N'Марина', N'Владимировна', CAST(N'1980-06-20' AS Date), 44, N'Женский', N'г. Тюмень, ул. Республики, д. 15, кв. 8', N'antonova@gmail.com', N'+70538901234', N'6227 890123', N'ООО "Фармацевтические Решения"', N'MC-00018', CAST(N'2026-06-12' AS Date), NULL, N'Согаз-Мед', N'8901234567890123', CAST(N'2026-06-17' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (19, N'901-234-567 90', N'Павлов', N'Сергей', N'Иванович', CAST(N'1994-09-05' AS Date), 30, N'Мужской', N'г. Ижевск, ул. Пушкинская, д. 10, кв. 3', N'pavlov@mail.ru', N'+70649012345', N'6328 901234', N'ПАО "Металлургический Комбинат"', N'MC-00019', CAST(N'2026-07-20' AS Date), NULL, N'АльфаСтрахование', N'9012345678901234', CAST(N'2026-07-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (20, N'012-345-678 91', N'Герасимова', N'Елена', N'Александровна', CAST(N'1986-12-10' AS Date), 38, N'Женский', N'г. Барнаул, ул. Ленина, д. 5, кв. 6', N'gerasimova@yandex.ru', N'+70750123456', N'6429 012345', N'ООО "Бухгалтерские Технологии"', N'MC-00020', CAST(N'2026-08-15' AS Date), NULL, N'Росгосстрах', N'0123456789012345', CAST(N'2026-08-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (21, N'123-456-789 03', N'Максимов', N'Александр', N'Сергеевич', CAST(N'1976-04-25' AS Date), 49, N'Мужской', N'г. Ульяновск, ул. Гончарова, д. 12, кв. 9', N'maksimov@gmail.com', N'+70861234567', N'6530 123456', N'АО "Телекоммуникационные Системы"', N'MC-00021', CAST(N'2026-09-22' AS Date), NULL, N'СОГАЗ-Мед', N'1234567890123456', CAST(N'2026-09-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (22, N'234-567-890 14', N'Захарова', N'Ольга', N'Игоревна', CAST(N'1990-07-30' AS Date), 34, N'Женский', N'г. Иркутск, ул. Ленина, д. 8, кв. 4', N'zakharova@mail.ru', N'+70972345678', N'6631 234567', N'ООО "Медицинские Услуги"', N'MC-00022', CAST(N'2026-10-10' AS Date), NULL, N'Ингосстрах', N'2345678901234567', CAST(N'2026-10-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (23, N'345-678-901 25', N'Кудрявцев', N'Дмитрий', N'Алексеевич', CAST(N'1983-02-14' AS Date), 42, N'Мужской', N'г. Хабаровск, ул. Муравьева-Амурского, д. 10, кв. 5', N'kudryavtsev@yandex.ru', N'+71083456789', N'6732 345678', N'ЗАО "Промышленные Инновации"', N'MC-00023', CAST(N'2026-11-18' AS Date), NULL, N'РЕСО-Мед', N'3456789012345678', CAST(N'2026-11-23' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (24, N'456-789-012 36', N'Белова', N'Анастасия', N'Сергеевна', CAST(N'1988-05-20' AS Date), 37, N'Женский', N'г. Ярославль, ул. Свободы, д. 15, кв. 7', N'belova@gmail.com', N'+71194567890', N'6833 456789', N'ООО "Фармацевтические Технологии"', N'MC-00024', CAST(N'2026-12-05' AS Date), NULL, N'Капитал Медицинское Страхование', N'4567890123456789', CAST(N'2026-12-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (25, N'567-890-123 47', N'Киселев', N'Алексей', N'Викторович', CAST(N'1975-08-15' AS Date), 49, N'Мужской', N'г. Владивосток, ул. Светланская, д. 20, кв. 12', N'kiselev@mail.ru', N'+71205678901', N'6934 567890', N'ПАО "Нефтегазовая Компания"', N'MC-00025', CAST(N'2027-01-15' AS Date), NULL, N'МАКС-М', N'5678901234567890', CAST(N'2027-01-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (26, N'678-901-234 58', N'Григорьева', N'Татьяна', N'Андреевна', CAST(N'1981-11-25' AS Date), 44, N'Женский', N'г. Махачкала, ул. Ленина, д. 5, кв. 3', N'grigorieva@yandex.ru', N'+71316789012', N'7035 678901', N'ООО "Торговые Решения"', N'MC-00026', CAST(N'2027-02-22' AS Date), NULL, N'Ренессанс Здоровье', N'6789012345678901', CAST(N'2027-02-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (27, N'789-012-345 69', N'Титов', N'Иван', N'Олегович', CAST(N'1992-03-10' AS Date), 33, N'Мужской', N'г. Оренбург, ул. Советская, д. 10, кв. 6', N'titov@gmail.com', N'+71427890123', N'7136 789012', N'АО "Строительные Инновации"', N'MC-00027', CAST(N'2027-03-08' AS Date), NULL, N'Согаз-Мед', N'7890123456789012', CAST(N'2027-03-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (28, N'890-123-456 80', N'Ковалева', N'Евгения', N'Дмитриевна', CAST(N'1984-06-15' AS Date), 41, N'Женский', N'г. Кемерово, ул. Весенняя, д. 8, кв. 4', N'kovaleva@mail.ru', N'+71538901234', N'7237 890123', N'ООО "Фармацевтические Исследования"', N'MC-00028', CAST(N'2027-04-15' AS Date), NULL, N'АльфаСтрахование', N'8901234567890123', CAST(N'2027-04-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (29, N'901-234-567 91', N'Никитин', N'Артем', N'Сергеевич', CAST(N'1978-09-20' AS Date), 46, N'Мужской', N'г. Рязань, ул. Ленина, д. 12, кв. 7', N'nikitin@yandex.ru', N'+71649012345', N'7338 901234', N'ПАО "Металлургические Технологии"', N'MC-00029', CAST(N'2027-05-22' AS Date), NULL, N'Росгосстрах', N'9012345678901234', CAST(N'2027-05-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (30, N'012-345-678 92', N'Соколова', N'Надежда', N'Александровна', CAST(N'1987-12-05' AS Date), 37, N'Женский', N'г. Астрахань, ул. Советская, д. 15, кв. 9', N'sokolova@gmail.com', N'+71750123456', N'7439 012345', N'ООО "Бухгалтерские Инновации"', N'MC-00030', CAST(N'2027-06-10' AS Date), NULL, N'СОГАЗ-Мед', N'0123456789012345', CAST(N'2027-06-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (31, N'123-456-789 04', N'Федоров', N'Максим', N'Игоревич', CAST(N'1980-02-18' AS Date), 45, N'Мужской', N'г. Пенза, ул. Московская, д. 20, кв. 11', N'fedorov@mail.ru', N'+71861234567', N'7540 123456', N'АО "Телекоммуникационные Решения"', N'MC-00031', CAST(N'2027-07-18' AS Date), NULL, N'Ингосстрах', N'1234567890123456', CAST(N'2027-07-23' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (32, N'234-567-890 15', N'Дмитриева', N'Юлия', N'Сергеевна', CAST(N'1993-05-25' AS Date), 32, N'Женский', N'г. Липецк, ул. Зегеля, д. 10, кв. 5', N'dmitrieva@yandex.ru', N'+71972345678', N'7641 234567', N'ООО "Медицинские Исследования"', N'MC-00032', CAST(N'2027-08-05' AS Date), NULL, N'РЕСО-Мед', N'2345678901234567', CAST(N'2027-08-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (33, N'345-678-901 26', N'Соловьев', N'Андрей', N'Алексеевич', CAST(N'1979-08-30' AS Date), 45, N'Мужской', N'г. Киров, ул. Ленина, д. 5, кв. 3', N'soloviev@gmail.com', N'+72083456789', N'7742 345678', N'ЗАО "Промышленные Ресурсы"', N'MC-00033', CAST(N'2027-09-12' AS Date), NULL, N'Капитал Медицинское Страхование', N'3456789012345678', CAST(N'2027-09-17' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (34, N'456-789-012 37', N'Воробьева', N'Екатерина', N'Олеговна', CAST(N'1985-11-15' AS Date), 39, N'Женский', N'г. Чебоксары, ул. Ленина, д. 10, кв. 6', N'vorobeva@mail.ru', N'+72194567890', N'7843 456789', N'ООО "Фармацевтические Продукты"', N'MC-00034', CAST(N'2027-10-20' AS Date), NULL, N'МАКС-М', N'4567890123456789', CAST(N'2027-10-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (35, N'567-890-123 48', N'Гусев', N'Виктор', N'Викторович', CAST(N'1974-02-20' AS Date), 51, N'Мужской', N'г. Калининград, ул. Гагарина, д. 15, кв. 8', N'gusev@yandex.ru', N'+72205678901', N'7944 567890', N'ПАО "Нефтехимическая Компания"', N'MC-00035', CAST(N'2027-11-08' AS Date), NULL, N'Ренессанс Здоровье', N'5678901234567890', CAST(N'2027-11-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (36, N'678-901-234 59', N'Титова', N'Марина', N'Андреевна', CAST(N'1982-05-25' AS Date), 43, N'Женский', N'г. Тула, ул. Советская, д. 8, кв. 4', N'titova@gmail.com', N'+72316789012', N'8045 678901', N'ООО "Торговые Технологии"', N'MC-00036', CAST(N'2027-12-15' AS Date), NULL, N'Согаз-Мед', N'6789012345678901', CAST(N'2027-12-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (37, N'789-012-345 70', N'Комаров', N'Сергей', N'Олегович', CAST(N'1991-08-10' AS Date), 33, N'Мужской', N'г. Ставрополь, ул. Ленина, д. 12, кв. 7', N'komarov@mail.ru', N'+72427890123', N'8146 789012', N'АО "Строительные Ресурсы"', N'MC-00037', CAST(N'2028-01-22' AS Date), NULL, N'АльфаСтрахование', N'7890123456789012', CAST(N'2028-01-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (38, N'890-123-456 81', N'Орлова', N'Анна', N'Владимировна', CAST(N'1983-11-15' AS Date), 41, N'Женский', N'г. Белгород, ул. Преображенская, д. 10, кв. 5', N'orlova@yandex.ru', N'+72538901234', N'8247 890123', N'ООО "Фармацевтические Инициативы"', N'MC-00038', CAST(N'2028-02-05' AS Date), NULL, N'Росгосстрах', N'8901234567890123', CAST(N'2028-02-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (39, N'901-234-567 92', N'Беляев', N'Денис', N'Сергеевич', CAST(N'1977-02-20' AS Date), 48, N'Мужской', N'г. Архангельск, ул. Ломоносова, д. 15, кв. 9', N'belyaev@gmail.com', N'+72649012345', N'8348 901234', N'ПАО "Металлургические Решения"', N'MC-00039', CAST(N'2028-03-12' AS Date), NULL, N'СОГАЗ-Мед', N'9012345678901234', CAST(N'2028-03-17' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (40, N'012-345-678 93', N'Горбунова', N'Ольга', N'Александровна', CAST(N'1989-05-25' AS Date), 36, N'Женский', N'г. Владимир, ул. Большая Московская, д. 10, кв. 6', N'gorbunova@mail.ru', N'+72750123456', N'8449 012345', N'ООО "Бухгалтерские Решения"', N'MC-00040', CAST(N'2028-04-20' AS Date), NULL, N'Ингосстрах', N'0123456789012345', CAST(N'2028-04-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (41, N'123-456-789 05', N'Данилов', N'Александр', N'Игоревич', CAST(N'1981-08-30' AS Date), 43, N'Мужской', N'г. Чита, ул. Ленина, д. 5, кв. 3', N'danilov@yandex.ru', N'+72861234567', N'8550 123456', N'АО "Телекоммуникационные Технологии"', N'MC-00041', CAST(N'2028-05-08' AS Date), NULL, N'РЕСО-Мед', N'1234567890123456', CAST(N'2028-05-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (42, N'234-567-890 16', N'Ефимова', N'Наталья', N'Сергеевна', CAST(N'1994-11-05' AS Date), 30, N'Женский', N'г. Смоленск, ул. Ленина, д. 8, кв. 4', N'efimova@gmail.com', N'+72972345678', N'8651 234567', N'ООО "Медицинские Технологии"', N'MC-00042', CAST(N'2028-06-15' AS Date), NULL, N'Капитал Медицинское Страхование', N'2345678901234567', CAST(N'2028-06-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (43, N'345-678-901 27', N'Жуков', N'Михаил', N'Алексеевич', CAST(N'1976-02-10' AS Date), 49, N'Мужской', N'г. Курск, ул. Ленина, д. 12, кв. 7', N'zhukov@mail.ru', N'+73083456789', N'8752 345678', N'ЗАО "Промышленные Инициативы"', N'MC-00043', CAST(N'2028-07-22' AS Date), NULL, N'МАКС-М', N'3456789012345678', CAST(N'2028-07-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (44, N'456-789-012 38', N'Зайцева', N'Елена', N'Олеговна', CAST(N'1987-05-15' AS Date), 48, N'Женский', N'г. Вологда, ул. Ленина, д. 10, кв. 5', N'zaytseva@yandex.ru', N'+73194567890', N'8853 456789', N'ООО "Фармацевтические Продукты"', N'MC-00044', CAST(N'2028-08-05' AS Date), NULL, N'Ренессанс Здоровье', N'4567890123456789', CAST(N'2028-08-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (45, N'567-890-123 49', N'Иванов', N'Алексей', N'Викторович', CAST(N'1973-08-20' AS Date), 51, N'Мужской', N'г. Саранск, ул. Советская, д. 15, кв. 8', N'ivanov.a@mail.ru', N'+73205678901', N'8954 567890', N'ПАО "Нефтегазовые Технологии"', N'MC-00045', CAST(N'2028-09-12' AS Date), NULL, N'Согаз-Мед', N'5678901234567890', CAST(N'2028-09-17' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (46, N'678-901-234 60', N'Климова', N'Татьяна', N'Андреевна', CAST(N'1980-11-25' AS Date), 44, N'Женский', N'г. Якутск, ул. Ленина, д. 8, кв. 4', N'klimova@gmail.com', N'+73316789012', N'9055 678901', N'ООО "Торговые Инициативы"', N'MC-00046', CAST(N'2028-10-20' AS Date), NULL, N'АльфаСтрахование', N'6789012345678901', CAST(N'2028-10-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (47, N'789-012-345 71', N'Ларин', N'Сергей', N'Олегович', CAST(N'1990-02-10' AS Date), 35, N'Мужской', N'г. Владикавказ, ул. Ленина, д. 12, кв. 7', N'larin@mail.ru', N'+73427890123', N'9156 789012', N'АО "Строительные Технологии"', N'MC-00047', CAST(N'2028-11-08' AS Date), NULL, N'Росгосстрах', N'7890123456789012', CAST(N'2028-11-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (48, N'890-123-456 82', N'Максимова', N'Анна', N'Владимировна', CAST(N'1982-05-15' AS Date), 43, N'Женский', N'г. Мурманск, ул. Ленина, д. 10, кв. 5', N'maximova@yandex.ru', N'+73538901234', N'9257 890123', N'ООО "Фармацевтические Исследования"', N'MC-00048', CAST(N'2028-12-15' AS Date), NULL, N'СОГАЗ-Мед', N'8901234567890123', CAST(N'2028-12-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (49, N'901-234-567 93', N'Новиков', N'Дмитрий', N'Сергеевич', CAST(N'1975-08-20' AS Date), 49, N'Мужской', N'г. Тверь, ул. Советская, д. 15, кв. 8', N'novikov.d@mail.ru', N'+73649012345', N'9358 901234', N'ПАО "Металлургические Инициативы"', N'MC-00049', CAST(N'2029-01-22' AS Date), NULL, N'Ингосстрах', N'9012345678901234', CAST(N'2029-01-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (50, N'012-345-678 94', N'Осипова', N'Ольга', N'Александровна', CAST(N'1988-11-25' AS Date), 36, N'Женский', N'г. Иваново, ул. Ленина, д. 8, кв. 4', N'osipova@gmail.com', N'+73750123456', N'9459 012345', N'ООО "Бухгалтерские Технологии"', N'MC-00050', CAST(N'2029-02-05' AS Date), NULL, N'РЕСО-Мед', N'0123456789012345', CAST(N'2029-02-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (51, N'123-456-789 06', N'Петров', N'Андрей', N'Игоревич', CAST(N'1980-02-10' AS Date), 45, N'Мужской', N'г. Брянск, ул. Ленина, д. 12, кв. 7', N'petrov.a@mail.ru', N'+73861234567', N'9560 123456', N'АО "Телекоммуникационные Решения"', N'MC-00051', CAST(N'2029-03-15' AS Date), NULL, N'Капитал Медицинское Страхование', N'1234567890123456', CAST(N'2029-03-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (52, N'234-567-890 17', N'Романова', N'Наталья', N'Сергеевна', CAST(N'1993-05-15' AS Date), 32, N'Женский', N'г. Томск, ул. Ленина, д. 10, кв. 5', N'romanova@yandex.ru', N'+73972345678', N'9661 234567', N'ООО "Медицинские Исследования"', N'MC-00052', CAST(N'2029-04-22' AS Date), NULL, N'МАКС-М', N'2345678901234567', CAST(N'2029-04-27' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (53, N'345-678-901 28', N'Сидоров', N'Дмитрий', N'Алексеевич', CAST(N'1977-08-20' AS Date), 47, N'Мужской', N'г. Кемерово, ул. Советская, д. 15, кв. 8', N'sidorov.d@mail.ru', N'+74083456789', N'9762 345678', N'ЗАО "Промышленные Ресурсы"', N'MC-00053', CAST(N'2029-05-10' AS Date), NULL, N'Ренессанс Здоровье', N'3456789012345678', CAST(N'2029-05-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (54, N'456-789-012 39', N'Тихонова', N'Екатерина', N'Олеговна', CAST(N'1985-11-25' AS Date), 39, N'Женский', N'г. Улан-Удэ, ул. Ленина, д. 8, кв. 4', N'tikhonova@gmail.com', N'+74194567890', N'9863 456789', N'ООО "Фармацевтические Продукты"', N'MC-00054', CAST(N'2029-06-18' AS Date), NULL, N'Согаз-Мед', N'4567890123456789', CAST(N'2029-06-23' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (55, N'567-890-123 50', N'Ушаков', N'Виктор', N'Викторович', CAST(N'1974-02-10' AS Date), 51, N'Мужской', N'г. Череповец, ул. Ленина, д. 12, кв. 7', N'ushakov@mail.ru', N'+74205678901', N'9964 567890', N'ПАО "Нефтехимические Технологии"', N'MC-00055', CAST(N'2029-07-05' AS Date), NULL, N'АльфаСтрахование', N'5678901234567890', CAST(N'2029-07-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (56, N'678-901-234 61', N'Федорова', N'Марина', N'Андреевна', CAST(N'1981-05-15' AS Date), 44, N'Женский', N'г. Орск, ул. Советская, д. 10, кв. 5', N'fedorova@yandex.ru', N'+743167890', N'9964 567890', N'ПАО "Нефтехимические Технологии"', N'MC-00055', CAST(N'2029-07-05' AS Date), NULL, N'АльфаСтрахование', N'5678901234567890', CAST(N'2029-07-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (57, N'123-456-789 01', N'Иванов', N'Иван', N'Иванович', CAST(N'1980-05-15' AS Date), 45, N'Мужской', N'г. Москва, ул. Ленина, д. 10, кв. 5', N'ivanov@mail.ru', N'+7(999)123-45-67', N'4510 123456', N'ООО "Технологии"', N'MC-001', CAST(N'2020-01-10' AS Date), NULL, N'Страховая Компания 1', N'1234567890123456', CAST(N'2020-01-05' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (58, N'234-567-890 12', N'Петрова', N'Мария', N'Сергеевна', CAST(N'1992-08-22' AS Date), 32, N'Женский', N'г. Санкт-Петербург, ул. Пушкина, д. 15', N'petrova@gmail.com', N'+7(911)234-56-78', N'4511 234567', N'ЗАО "Промышленность"', N'MC-002', CAST(N'2019-11-15' AS Date), NULL, N'Страховая Компания 2', N'2345678901234567', CAST(N'2019-11-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (59, N'345-678-901 23', N'Сидоров', N'Алексей', NULL, CAST(N'1975-03-30' AS Date), 49, N'Мужской', N'г. Новосибирск, пр. Карла Маркса, д. 33', N'sidorov@yandex.ru', N'+7(912)345-67-89', N'4512 345678', N'ИП Сидоров А.В.', N'MC-003', CAST(N'2021-02-20' AS Date), NULL, N'Страховая Компания 3', N'3456789012345678', CAST(N'2021-02-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (60, N'456-789-012 34', N'Кузнецова', N'Елена', N'Владимировна', CAST(N'1988-11-12' AS Date), 36, N'Женский', N'г. Екатеринбург, ул. Мира, д. 7, кв. 12', N'kuznetsova@mail.ru', N'+7(913)456-78-90', N'4513 456789', N'ОАО "Металлург"', N'MC-004', CAST(N'2022-03-25' AS Date), NULL, N'Страховая Компания 1', N'4567890123456789', CAST(N'2022-03-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (61, N'567-890-123 45', N'Васильев', N'Дмитрий', N'Петрович', CAST(N'1995-07-18' AS Date), 29, N'Мужской', N'г. Казань, ул. Гагарина, д. 25', N'vasilyev@gmail.com', N'+7(914)567-89-01', N'4514 567890', N'ПАО "Банк"', N'MC-005', CAST(N'2023-04-05' AS Date), NULL, N'Страховая Компания 2', N'5678901234567890', CAST(N'2023-04-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (62, N'678-901-234 56', N'Смирнова', N'Ольга', N'Александровна', CAST(N'1983-09-05' AS Date), 41, N'Женский', N'г. Нижний Новгород, ул. Горького, д. 12, кв. 3', N'smirnova@yandex.ru', N'+7(915)678-90-12', N'4515 678901', N'ООО "СтройТех"', N'MC-006', CAST(N'2021-05-15' AS Date), NULL, N'Страховая Компания 3', N'6789012345678901', CAST(N'2021-05-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (63, N'789-012-345 67', N'Попов', N'Андрей', N'Игоревич', CAST(N'1990-12-25' AS Date), 34, N'Мужской', N'г. Самара, ул. Куйбышева, д. 45', N'popov@mail.ru', N'+7(916)789-01-23', N'4516 789012', N'ИП Попов А.И.', N'MC-007', CAST(N'2020-07-20' AS Date), NULL, N'Страховая Компания 1', N'7890123456789012', CAST(N'2020-07-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (64, N'890-123-456 78', N'Федорова', N'Анна', N'Дмитриевна', CAST(N'1978-04-17' AS Date), 46, N'Женский', N'г. Омск, ул. Лермонтова, д. 8, кв. 7', N'fedorova@gmail.com', N'+7(917)890-12-34', N'4517 890123', N'ООО "Торговая Компания"', N'MC-008', CAST(N'2022-08-30' AS Date), NULL, N'Страховая Компания 2', N'8901234567890123', CAST(N'2022-08-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (65, N'901-234-567 89', N'Николаев', N'Сергей', NULL, CAST(N'1987-06-30' AS Date), 37, N'Мужской', N'г. Ростов-на-Дону, ул. Советская, д. 22', N'nikolaev@yandex.ru', N'+7(918)901-23-45', N'4518 901234', N'ПАО "ЭнергоСбыт"', N'MC-009', CAST(N'2023-01-12' AS Date), NULL, N'Страховая Компания 3', N'9012345678901234', CAST(N'2023-01-07' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (66, N'012-345-678 90', N'Козлова', N'Татьяна', N'Викторовна', CAST(N'1993-02-14' AS Date), 31, N'Женский', N'г. Уфа, ул. Революционная, д. 5, кв. 9', N'kozlova@mail.ru', N'+7(919)012-34-56', N'4519 012345', N'ООО "Фармацевтика"', N'MC-010', CAST(N'2021-09-18' AS Date), NULL, N'Страховая Компания 1', N'0123456789012345', CAST(N'2021-09-13' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (67, N'112-233-445 56', N'Морозов', N'Артём', N'Владимирович', CAST(N'1991-04-12' AS Date), 33, N'Мужской', N'г. Воронеж, ул. Кирова, д. 18, кв. 9', N'morozov.a@mail.ru', N'+7(920)111-22-33', N'4520 112233', N'ООО "СтройГрад"', N'MC-011', CAST(N'2022-06-14' AS Date), NULL, N'Страховая Компания 1', N'1122334455667788', CAST(N'2022-06-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (68, N'223-344-556 67', N'Захарова', N'Виктория', N'Олеговна', CAST(N'1986-07-25' AS Date), 38, N'Женский', N'г. Краснодар, ул. Красная, д. 33', N'zakharova.v@yandex.ru', N'+7(921)222-33-44', N'4521 223344', N'ПАО "ЮгТранс"', N'MC-012', CAST(N'2021-09-05' AS Date), NULL, N'Страховая Компания 2', N'2233445566778899', CAST(N'2021-08-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (69, N'334-455-667 78', N'Белов', N'Константин', NULL, CAST(N'1979-11-30' AS Date), 45, N'Мужской', N'г. Пермь, ул. Ленина, д. 47', N'belov.k@gmail.com', N'+7(922)333-44-55', N'4522 334455', N'ИП Белов К.Н.', N'MC-013', CAST(N'2023-03-18' AS Date), NULL, N'Страховая Компания 3', N'3344556677889900', CAST(N'2023-03-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (70, N'445-566-778 89', N'Григорьева', N'Алина', N'Игоревна', CAST(N'1994-02-14' AS Date), 31, N'Женский', N'г. Тюмень, ул. Республики, д. 15, кв. 6', N'grigoreva.a@mail.ru', N'+7(923)444-55-66', N'4523 445566', N'ООО "НефтеГазСервис"', N'MC-014', CAST(N'2020-12-10' AS Date), NULL, N'Страховая Компания 1', N'4455667788990011', CAST(N'2020-12-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (71, N'556-677-889 90', N'Титов', N'Роман', N'Анатольевич', CAST(N'1983-09-08' AS Date), 41, N'Мужской', N'г. Иркутск, ул. Декабристов, д. 22', N'titov.r@yandex.ru', N'+7(924)555-66-77', N'4524 556677', N'АО "БайкалЭнерго"', N'MC-015', CAST(N'2021-07-22' AS Date), NULL, N'Страховая Компания 2', N'5566778899001122', CAST(N'2021-07-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (72, N'667-788-990 01', N'Крылова', N'Юлия', N'Сергеевна', CAST(N'1989-05-19' AS Date), 35, N'Женский', N'г. Ульяновск, ул. Гончарова, д. 10, кв. 4', N'krylova.yu@gmail.com', N'+7(925)666-77-88', N'4525 667788', N'ООО "АвиаСтар"', N'MC-016', CAST(N'2022-04-30' AS Date), NULL, N'Страховая Компания 3', N'6677889900112233', CAST(N'2022-04-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (73, N'778-899-001 12', N'Фомин', N'Павел', N'Денисович', CAST(N'1976-12-03' AS Date), 48, N'Мужской', N'г. Владивосток, ул. Светланская, д. 55', N'fomin.p@mail.ru', N'+7(926)777-88-99', N'4526 778899', N'ОАО "ДальРыба"', N'MC-017', CAST(N'2020-10-15' AS Date), NULL, N'Страховая Компания 1', N'7788990011223344', CAST(N'2020-10-05' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (74, N'889-900-112 23', N'Медведева', N'Екатерина', N'Валерьевна', CAST(N'1997-08-27' AS Date), 27, N'Женский', N'г. Хабаровск, ул. Ленина, д. 72, кв. 11', N'medvedeva.e@yandex.ru', N'+7(927)888-99-00', N'4527 889900', N'ЗАО "Дальневосточные Ресурсы"', N'MC-018', CAST(N'2023-01-08' AS Date), NULL, N'Страховая Компания 2', N'8899001122334455', CAST(N'2023-01-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (75, N'990-011-223 34', N'Кузьмин', N'Антон', NULL, CAST(N'1980-03-21' AS Date), 44, N'Мужской', N'г. Ярославль, ул. Свободы, д. 30', N'kuzmin.a@gmail.com', N'+7(928)999-00-11', N'4528 990011', N'ИП Кузьмин А.В.', N'MC-019', CAST(N'2021-11-19' AS Date), NULL, N'Страховая Компания 3', N'9900112233445566', CAST(N'2021-11-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (76, N'001-122-334 45', N'Орлова', N'Наталья', N'Викторовна', CAST(N'1992-06-09' AS Date), 32, N'Женский', N'г. Томск, ул. Белая, д. 8, кв. 3', N'orlova.n@mail.ru', N'+7(929)000-11-22', N'4529 001122', N'ООО "Научные Технологии"', N'MC-020', CAST(N'2022-09-25' AS Date), NULL, N'Страховая Компания 1', N'0011223344556677', CAST(N'2022-09-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (77, N'102-938-475 61', N'Соловьёв', N'Максим', N'Андреевич', CAST(N'1985-01-30' AS Date), 39, N'Мужской', N'г. Калининград, ул. Гагарина, д. 14, кв. 7', N'soloviev.m@mail.ru', N'+7(931)234-56-78', N'4530 102938', N'ООО "БалтСтрой"', N'MC-041', CAST(N'2021-05-12' AS Date), NULL, N'Страховая Компания 2', N'1029384756617283', CAST(N'2021-05-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (78, N'203-849-506 72', N'Виноградова', N'Анжела', N'Борисовна', CAST(N'1990-09-15' AS Date), 34, N'Женский', N'г. Сочи, ул. Навагинская, д. 25', N'vinogradova.a@yandex.ru', N'+7(932)345-67-89', N'4531 203849', N'ОАО "КурортСервис"', N'MC-042', CAST(N'2022-08-20' AS Date), NULL, N'Страховая Компания 3', N'2038495067728394', CAST(N'2022-08-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (79, N'304-950-617 83', N'Герасимов', N'Игорь', NULL, CAST(N'1977-04-22' AS Date), 47, N'Мужской', N'г. Волгоград, пр. Ленина, д. 33', N'gerasimov.i@gmail.com', N'+7(933)456-78-90', N'4532 304950', N'ИП Герасимов И.Л.', N'MC-043', CAST(N'2020-11-05' AS Date), NULL, N'Страховая Компания 1', N'3049506177839405', CAST(N'2020-10-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (80, N'405-061-728 94', N'Данилова', N'Олеся', N'Витальевна', CAST(N'1988-07-08' AS Date), 36, N'Женский', N'г. Саратов, ул. Московская, д. 42, кв. 15', N'danilova.o@mail.ru', N'+7(934)567-89-01', N'4533 405061', N'ПАО "ВолгаБанк"', N'MC-044', CAST(N'2023-02-18' AS Date), NULL, N'Страховая Компания 2', N'4050617288940516', CAST(N'2023-02-08' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (81, N'506-172-839 05', N'Ермаков', N'Денис', N'Сергеевич', CAST(N'1993-12-11' AS Date), 31, N'Мужской', N'г. Тверь, ул. Советская, д. 18', N'ermakov.d@yandex.ru', N'+7(935)678-90-12', N'4534 506172', N'ООО "ТверьХимПром"', N'MC-045', CAST(N'2021-09-30' AS Date), NULL, N'Страховая Компания 3', N'5061728390051627', CAST(N'2021-09-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (82, N'607-283-940 16', N'Жукова', N'Вероника', N'Алексеевна', CAST(N'1982-03-25' AS Date), 42, N'Женский', N'г. Ставрополь, ул. Дзержинского, д. 7, кв. 9', N'zhukova.v@gmail.com', N'+7(936)789-01-23', N'4535 607283', N'ЗАО "КавказАгро"', N'MC-046', CAST(N'2022-07-14' AS Date), NULL, N'Страховая Компания 1', N'6072839401162738', CAST(N'2022-07-04' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (83, N'708-394-051 27', N'Зайцев', N'Артур', N'Романович', CAST(N'1974-06-19' AS Date), 50, N'Мужской', N'г. Владимир, ул. Большая Московская, д. 56', N'zaitsev.a@mail.ru', N'+7(937)890-12-34', N'4536 708394', N'ООО "АвтоПромСнаб"', N'MC-047', CAST(N'2020-04-22' AS Date), NULL, N'Страховая Компания 2', N'7083940512273849', CAST(N'2020-04-12' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (84, N'809-405-162 38', N'Исаева', N'Диана', NULL, CAST(N'1996-10-03' AS Date), 28, N'Женский', N'г. Чита, ул. Ленина, д. 29, кв. 5', N'isaeva.d@yandex.ru', N'+7(938)901-23-45', N'4537 809405', N'ФГБУ "ЗабайкалМед"', N'MC-048', CAST(N'2023-05-10' AS Date), NULL, N'Страховая Компания 3', N'8094051623384950', CAST(N'2023-05-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (85, N'910-516-273 49', N'Ковалёв', N'Станислав', N'Викторович', CAST(N'1981-02-28' AS Date), 43, N'Мужской', N'г. Белгород, ул. Преображенская, д. 33', N'kovalev.s@gmail.com', N'+7(939)012-34-56', N'4538 910516', N'АО "Белэнергомаш"', N'MC-049', CAST(N'2021-12-05' AS Date), NULL, N'Страховая Компания 1', N'9105162734495061', CAST(N'2021-11-25' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (86, N'021-627-384 50', N'Ларина', N'Элина', N'Олеговна', CAST(N'1987-05-14' AS Date), 37, N'Женский', N'г. Архангельск, ул. Воскресенская, д. 12, кв. 8', N'larina.e@mail.ru', N'+7(940)123-45-67', N'4539 021627', N'ООО "СеверЛес"', N'MC-050', CAST(N'2022-10-28' AS Date), NULL, N'Страховая Компания 2', N'0216273845506172', CAST(N'2022-10-18' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (87, N'132-547-689 01', N'Миронов', N'Глеб', N'Артёмович', CAST(N'1984-08-17' AS Date), 40, N'Мужской', N'г. Курск, ул. Ленина, д. 45, кв. 12', N'mironov.g@mail.ru', N'+7(950)234-56-78', N'4540 132547', N'ООО "КурскПромСтрой"', N'MC-051', CAST(N'2022-03-15' AS Date), NULL, N'Страховая Компания 3', N'1325476890123456', CAST(N'2022-03-05' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (88, N'243-658-790 12', N'Новикова', N'Арина', N'Денисовна', CAST(N'1995-11-23' AS Date), 29, N'Женский', N'г. Орёл, ул. Московская, д. 28', N'novikova.a@yandex.ru', N'+7(951)345-67-89', N'4541 243658', N'ПАО "ОрёлБанк"', N'MC-052', CAST(N'2023-01-20' AS Date), NULL, N'Страховая Компания 1', N'2436587901234567', CAST(N'2023-01-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (89, N'354-769-801 23', N'Осипов', N'Вадим', NULL, CAST(N'1979-02-14' AS Date), 46, N'Мужской', N'г. Смоленск, ул. Багратиона, д. 17', N'osipov.v@gmail.com', N'+7(952)456-78-90', N'4542 354769', N'ИП Осипов В.Р.', N'MC-053', CAST(N'2021-07-12' AS Date), NULL, N'Страховая Компания 2', N'3547698012345678', CAST(N'2021-07-02' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (90, N'465-870-912 34', N'Павлова', N'Ксения', N'Ильинична', CAST(N'1986-05-30' AS Date), 37, N'Женский', N'г. Мурманск, ул. Ленинградская, д. 5, кв. 3', N'pavlova.k@mail.ru', N'+7(953)567-89-01', N'4543 465870', N'ОАО "МурманРыбПром"', N'MC-054', CAST(N'2020-09-25' AS Date), NULL, N'Страховая Компания 3', N'4658709123456789', CAST(N'2020-09-15' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (91, N'576-981-023 45', N'Родионов', N'Тимур', N'Олегович', CAST(N'1992-09-07' AS Date), 32, N'Мужской', N'г. Иваново, ул. 8 Марта, д. 22', N'rodionov.t@yandex.ru', N'+7(954)678-90-12', N'4544 576981', N'ООО "ИвТекстиль"', N'MC-055', CAST(N'2023-04-18' AS Date), NULL, N'Страховая Компания 1', N'5769810234567890', CAST(N'2023-04-08' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (92, N'687-092-134 56', N'Савельева', N'Яна', N'Вячеславовна', CAST(N'1983-12-12' AS Date), 41, N'Женский', N'г. Брянск, ул. Красноармейская, д. 33, кв. 7', N'savelieva.y@gmail.com', N'+7(955)789-01-23', N'4545 687092', N'ЗАО "БрянскМаш"', N'MC-056', CAST(N'2021-11-30' AS Date), NULL, N'Страховая Компания 2', N'6870921345678901', CAST(N'2021-11-20' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (93, N'798-103-245 67', N'Тарасов', N'Евгений', N'Геннадьевич', CAST(N'1975-04-05' AS Date), 49, N'Мужской', N'г. Кемерово, пр. Октябрьский, д. 48', N'tarasov.e@mail.ru', N'+7(956)890-12-34', N'4546 798103', N'АО "КузбассУголь"', N'MC-057', CAST(N'2020-06-22' AS Date), NULL, N'Страховая Компания 3', N'7981032456789012', CAST(N'2020-06-12' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (94, N'809-214-356 78', N'Ушакова', N'Лилия', NULL, CAST(N'1998-07-19' AS Date), 26, N'Женский', N'г. Тамбов, ул. Советская, д. 15, кв. 6', N'ushakova.l@yandex.ru', N'+7(957)901-23-45', N'4547 809214', N'ООО "ТамбовАгро"', N'MC-058', CAST(N'2023-02-14' AS Date), NULL, N'Страховая Компания 1', N'8092143567890123', CAST(N'2023-02-04' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (95, N'910-325-467 89', N'Фёдоров', N'Даниил', N'Анатольевич', CAST(N'1989-01-25' AS Date), 35, N'Мужской', N'г. Липецк, ул. Зегеля, д. 27', N'fedorov.d@gmail.com', N'+7(958)012-34-56', N'4548 910325', N'ПАО "НЛМК"', N'MC-059', CAST(N'2022-05-10' AS Date), NULL, N'Страховая Компания 2', N'9103254678901234', CAST(N'2022-05-01' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (96, N'021-436-578 90', N'Харитонова', N'Евгения', N'Сергеевна', CAST(N'1980-10-08' AS Date), 44, N'Женский', N'г. Пенза, ул. Кирова, д. 11, кв. 4', N'kharitonova.e@mail.ru', N'+7(959)123-45-67', N'4549 021436', N'ОАО "ПензаХлеб"', N'MC-060', CAST(N'2021-08-05' AS Date), NULL, N'Страховая Компания 3', N'0214365789012345', CAST(N'2021-07-26' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (97, N'132-547-689 02', N'Цветков', N'Арсений', N'Вадимович', CAST(N'1994-03-31' AS Date), 30, N'Мужской', N'г. Рязань, ул. Почтовая, д. 19', N'tsvetkov.a@yandex.ru', N'+7(960)234-56-78', N'4550 132547', N'ООО "РязаньАвто"', N'MC-061', CAST(N'2023-06-20' AS Date), NULL, N'Страховая Компания 1', N'1325476890234567', CAST(N'2023-06-10' AS Date))
GO
INSERT [dbo].[patient] ([id_patient], [snils], [lastname], [name], [surname], [birthday], [age], [gender], [address], [email], [phone], [passport_number_and_series], [place_of_work], [number_med_card], [date_out_med_card], [photo], [name_insurance_company], [number_policy_OMS], [date_and_insur_policy]) VALUES (98, N'243-658-790 13', N'Шестакова', N'Алёна', N'Андреевна', CAST(N'1987-06-14' AS Date), 37, N'Женский', N'г. Астрахань, ул. Советская, д. 24, кв. 9', N'shestakova.a@gmail.com', N'+7(961)345-67-89', N'4551 243658', N'ИП Шестакова А.А.', N'MC-062', CAST(N'2022-09-15' AS Date), NULL, N'Страховая Компания 2', N'2436587901345678', CAST(N'2022-09-05' AS Date))
GO
SET IDENTITY_INSERT [dbo].[patient] OFF
GO
SET IDENTITY_INSERT [dbo].[prescription_form_107у] ON 
GO
INSERT [dbo].[prescription_form_107у] ([number], [id_inspection], [date], [name_of_the_drug], [dosage], [method_of_administration], [method_of_administration_details], [dosage_regimen], [dosage_regimen_optional], [duration_of_treatment_number], [duration_of_treatment_duration], [duration_of_treatment_comments], [justification_of_appointment]) VALUES (1, 15, CAST(N'0001-01-01' AS Date), N'абакавир', N'10 г, 2 раза в день, Внутривенно', N'Подкожно', N'', N'По требованию', N'', N'4', N'Недели', N'', N'')
GO
SET IDENTITY_INSERT [dbo].[prescription_form_107у] OFF
GO
SET IDENTITY_INSERT [dbo].[referral] ON 
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (1, 1, N'На консультацию', CAST(N'2025-04-11' AS Date), N'ОМС', N'Визуальное исследоание в дерматологии', NULL, N'Киселева Алина Александровна', CAST(N'2025-05-12' AS Date), NULL)
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (2, 8, N'Консультация гастроэнтеролога', NULL, N'ОМС', N'A01.01.004 - Сбор анамнеза и жалоб в косметологии', N'Детская клиническая больница', N'Петрова Е.С. (кардиолог)', CAST(N'2025-05-18' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (3, 11, N'Консультация гастроэнтеролога', NULL, N'ОМС', N'A01.01.004 - Сбор анамнеза и жалоб в косметологии', N'Стоматологическая поликлиника', N'Васильева Т.К. (офтальмолог)', CAST(N'2025-05-24' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (4, 12, N'Консультация ревматолога', NULL, N'ОМС', N'A01.01.004 - Сбор анамнеза и жалоб в косметологии', N'Городская больница №1', N'Смирнов Д.А. (хирург)', CAST(N'2025-05-23' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (5, 13, N'Консультация гастроэнтеролога', NULL, N'ОМС', N'A01.01.003.001 - Пальпация при термических, химических и электрических ожогах', N'Областной онкологический диспансер', N'Кузнецова О.П. (педиатр)', CAST(N'2025-05-22' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (6, 13, N'Перевязка', NULL, N'ОМС', N'A01.01.003 - Пальпация в дерматологии', N'Поликлиника №3', N'Петрова Е.С. (кардиолог)', CAST(N'2025-05-17' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (7, 13, N'Консультация невролога', NULL, N'ОМС', N'A01.01.003.001 - Пальпация при термических, химических и электрических ожогах', N'Областной онкологический диспансер', N'', CAST(N'2025-05-24' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (8, 13, N'Консультация эндокринолога', NULL, N'ОМС', N'A01.01.003.001 - Пальпация при термических, химических и электрических ожогах', N'Стоматологическая поликлиника', N'', CAST(N'2025-05-13' AS Date), N'')
GO
INSERT [dbo].[referral] ([id_referral], [id_inspection], [type_of_direction], [date_of_creation], [payment_source], [service], [organization], [doctor], [date_of_admission], [justification]) VALUES (9, 14, N'Перевязка', NULL, N'ОМС', N'A01.01.003.001 - Пальпация при термических, химических и электрических ожогах', N'Стоматологическая поликлиника', N'', CAST(N'2025-05-13' AS Date), N'')
GO
SET IDENTITY_INSERT [dbo].[referral] OFF
GO
SET IDENTITY_INSERT [dbo].[specialization] ON 
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (1, N'Абдоминальный хирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (2, N'Аллерголог-иммунолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (3, N'Акушер-гинеколог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (4, N'Анестезиолог-реаниматолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (5, N'Андролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (6, N'Бактериолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (7, N'Гастроэнтеролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (8, N'Гематолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (9, N'Генетик')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (10, N'Гериатр')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (11, N'Гинеколог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (12, N'Дерматовенеролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (13, N'Диетолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (14, N'Инфекционист')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (15, N'Кардиолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (16, N'Кардиохирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (17, N'Колопроктолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (18, N'Лабораторный генетик')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (19, N'ЛОР (оториноларинголог)')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (20, N'Маммолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (21, N'Нарколог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (22, N'Невролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (23, N'Нейрохирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (24, N'Неонатолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (25, N'Нефролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (26, N'Онколог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (27, N'Офтальмолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (28, N'Паразитолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (29, N'Патологоанатом')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (30, N'Педиатр')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (31, N'Пластическийхирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (32, N'Проктолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (33, N'Психиатр')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (34, N'Психотерапевт')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (35, N'Пульмонолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (36, N'Радиолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (37, N'Ревматолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (38, N'Реаниматолог ')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (39, N'Рефлексотерапевт')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (40, N'Семейный врач')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (41, N'Сомнолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (42, N'Стоматолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (43, N'Стоматолог-ортодонт')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (44, N'Стоматолог-хирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (45, N'Сурдолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (46, N'Терапевт')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (47, N'Торакальный хирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (48, N'Травматолог-ортопед')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (49, N'Трансфузиолог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (50, N'УЗИ-специалист')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (51, N'Уролог')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (52, N'Фтизиатр')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (53, N'Челюстно-лицевой хирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (54, N'Хирург')
GO
INSERT [dbo].[specialization] ([id_specialization], [name]) VALUES (55, N'Эндокринолог')
GO
SET IDENTITY_INSERT [dbo].[specialization] OFF
GO
ALTER TABLE [dbo].[attachment]  WITH CHECK ADD  CONSTRAINT [FK_attachment_hospital] FOREIGN KEY([id_hospital])
REFERENCES [dbo].[hospital] ([id_hospital])
GO
ALTER TABLE [dbo].[attachment] CHECK CONSTRAINT [FK_attachment_hospital]
GO
ALTER TABLE [dbo].[attachment]  WITH CHECK ADD  CONSTRAINT [FK_attachment_patient] FOREIGN KEY([id_patient])
REFERENCES [dbo].[patient] ([id_patient])
GO
ALTER TABLE [dbo].[attachment] CHECK CONSTRAINT [FK_attachment_patient]
GO
ALTER TABLE [dbo].[disability_certificate]  WITH CHECK ADD  CONSTRAINT [FK_disability_certificate_inspection] FOREIGN KEY([id_inspection])
REFERENCES [dbo].[inspection] ([id_inspection])
GO
ALTER TABLE [dbo].[disability_certificate] CHECK CONSTRAINT [FK_disability_certificate_inspection]
GO
ALTER TABLE [dbo].[doctor]  WITH CHECK ADD  CONSTRAINT [FK_doctor_specialization] FOREIGN KEY([id_specialization])
REFERENCES [dbo].[specialization] ([id_specialization])
GO
ALTER TABLE [dbo].[doctor] CHECK CONSTRAINT [FK_doctor_specialization]
GO
ALTER TABLE [dbo].[inspection]  WITH CHECK ADD  CONSTRAINT [FK_inspection_doctor] FOREIGN KEY([id_doctor])
REFERENCES [dbo].[doctor] ([id_doctor])
GO
ALTER TABLE [dbo].[inspection] CHECK CONSTRAINT [FK_inspection_doctor]
GO
ALTER TABLE [dbo].[inspection]  WITH CHECK ADD  CONSTRAINT [FK_inspection_patient] FOREIGN KEY([id_patient])
REFERENCES [dbo].[patient] ([id_patient])
GO
ALTER TABLE [dbo].[inspection] CHECK CONSTRAINT [FK_inspection_patient]
GO
ALTER TABLE [dbo].[period]  WITH CHECK ADD  CONSTRAINT [FK_period_disability_certificate] FOREIGN KEY([id_disability_certificate])
REFERENCES [dbo].[disability_certificate] ([id_disability_certificate])
GO
ALTER TABLE [dbo].[period] CHECK CONSTRAINT [FK_period_disability_certificate]
GO
ALTER TABLE [dbo].[prescription_form_107у]  WITH CHECK ADD  CONSTRAINT [FK_prescription_form_107у_inspection] FOREIGN KEY([id_inspection])
REFERENCES [dbo].[inspection] ([id_inspection])
GO
ALTER TABLE [dbo].[prescription_form_107у] CHECK CONSTRAINT [FK_prescription_form_107у_inspection]
GO
ALTER TABLE [dbo].[referral]  WITH CHECK ADD  CONSTRAINT [FK_referral_inspection] FOREIGN KEY([id_inspection])
REFERENCES [dbo].[inspection] ([id_inspection])
GO
ALTER TABLE [dbo].[referral] CHECK CONSTRAINT [FK_referral_inspection]
GO
ALTER TABLE [dbo].[representative_patient]  WITH CHECK ADD  CONSTRAINT [FK_representative_patient_patient] FOREIGN KEY([id_patient])
REFERENCES [dbo].[patient] ([id_patient])
GO
ALTER TABLE [dbo].[representative_patient] CHECK CONSTRAINT [FK_representative_patient_patient]
GO
USE [master]
GO
ALTER DATABASE [MedCardDB] SET  READ_WRITE 
GO
