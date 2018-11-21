﻿using DeployCmsData.Test.Builders;
using Moq;
using NUnit.Framework;
using System;

namespace DeployCmsData.Test.Tests
{
    [TestFixture]
    public static class CreateNewDocumentTypeFolder
    {
        [Test]
        public static void CreateFolderAtRoot()
        {
            const string folderName = "My New Folder";

            var setup = new DocumentTypeFolderTestBuilder();
            var builder = setup
                .ReturnsNewFolder(folderName)
                .Build();

            var folder = builder
                .Name(folderName)
                .BuildAtRoot();

            setup.UmbracoFactory.Verify(x => x.NewContainer(UmbracoCms.Constants.Umbraco.RootFolder, folderName, 1), Times.Once);
            Assert.IsNotNull(folder);
            Assert.AreEqual(folderName, folder.Name);
        }

        [Test]
        public static void CreateFolderWithParentFolderAndLevel()
        {
            const string folderName = "My new folder";
            const string parentFolderName = "Parent folder name";
            const int parentFolderLevel = 7;

            var setup = new DocumentTypeFolderTestBuilder();
            var builder = setup
                .ReturnsNewFolder(folderName)
                .ReturnsExistingFolder(parentFolderName, parentFolderLevel)
                .Build();

            var folder = builder
                .Name(folderName)
                .BuildWithParentFolder(parentFolderName, parentFolderLevel);

            setup.UmbracoFactory.Verify(x => x.NewContainer(It.IsAny<int>(), folderName, parentFolderLevel), Times.Once);
            Assert.IsNotNull(folder);
            Assert.AreEqual(folderName, folder.Name);
        }

        [Test]
        public static void CreateFolderWithParentFolderAndNoLevel()
        {
            const string folderName = "My new folder";
            const string parentFolderName = "Parent folder name";
            const int parentFolderLevel = 7;

            var setup = new DocumentTypeFolderTestBuilder();
            var builder = setup
                .ReturnsNewFolder(folderName)
                .ReturnsExistingFolder(parentFolderName, parentFolderLevel)
                .Build();

            var folder = builder
                .Name(folderName)
                .BuildWithParentFolder(parentFolderName);

            setup.UmbracoFactory.Verify(x => x.NewContainer(It.IsAny<int>(), folderName, parentFolderLevel), Times.Once);
            Assert.IsNotNull(folder);
        }

        [Test]
        public static void DocumentTypeFolderCreateWithInvalidParentAndLevel()
        {
            const string folderName = "My new folder";
            const string parentFolderName = "Parent folder name";

            var setup = new DocumentTypeFolderTestBuilder();
            var builder = setup
                .ReturnsNewFolder(folderName)
                .Build();

            Assert.Throws<ArgumentException>(() =>
                builder.
                    Name(folderName).
                    BuildWithParentFolder(parentFolderName, 1));
        }

        [Test]
        public static void DocumentTypeFolderCreateWithInvalidParentNoLevel()
        {
            const string folderName = "My new folder";
            const string parentFolderName = "Parent folder name";

            var setup = new DocumentTypeFolderTestBuilder();
            var builder = setup
                .ReturnsNewFolder(folderName)
                .Build();

            Assert.Throws<ArgumentException>(() =>
                builder.
                    Name(folderName).
                    BuildWithParentFolder(parentFolderName));
           
            setup.UmbracoFactory.Verify(x =>
                    x.GetContainer(
                        parentFolderName,
                        It.IsAny<int>()),
                Times.Exactly(UmbracoCms.Constants.Umbraco.MaximumFolderLevel));
        }
    }
}