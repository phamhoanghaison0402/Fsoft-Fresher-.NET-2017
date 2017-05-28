using System;
using System.Collections.Generic;
using System.Linq;
using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace IPM.UnitTest.BusinessTest
{
    [TestClass]
    public class SkillBusinessTest
    {
        private Mock<ISkillRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ISkillBusiness _skillBusiness;
        private List<Skill> _listskill;


        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<ISkillRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _skillBusiness = new SkillBusiness(_mockRepository.Object, _mockUnitOfWork.Object);
            _listskill = new List<Skill>()
            {
                new Skill() {ID = 1 ,Name="Skill1", Active = true},
                new Skill() {ID = 2 ,Name="Skill2", Active = true},
                new Skill() {ID = 3 ,Name="Skill3", Active = true}
            };
        }

        [TestMethod]
        public void GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.Get(x => x.Active == true, null, "")).Returns(_listskill);

            //call action
            var result = _skillBusiness.GetAll() as List<Skill>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }


        [TestMethod]
        public void Search()
        {
            //setup method
            string name = "test";
            _mockRepository.Setup(m => m.Search(name)).Returns(_listskill);

            //call action
            var result = _skillBusiness.Search(name) as List<Skill>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Insert()
        {
            //setup method
            Skill skill = new Skill();
            skill.ID = 8;
            _mockRepository.Setup(m => m.Add(skill)).Returns(skill);

            //call action
            var result = _skillBusiness.Insert(skill);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.ID);
        }

        [TestMethod]
        public void Update()
        {
            //setup method
            Skill skill = new Skill();
            skill.ID = 8;
            skill.Active = false;
            skill.Name = "test";
            _mockRepository.Setup(m => m.Update(skill));

            //call action
            _skillBusiness.Update(skill);

            //compare
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void Delete()
        {
            //setup method
            Skill skill = new Skill();
            skill.ID = 8;
            skill.Active = false;
            skill.Name = "test";
            _mockRepository.Setup(m => m.GetSingleById(8)).Returns(skill);
            _mockRepository.Setup(m => m.Update(skill));

            //call action
            var result = _skillBusiness.Delete(8);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(skill, result);
        }

        [TestMethod]
        public void CheckName()
        {
            //setup method
            string name = "Skill";
            List<Skill> _listskill2 = new List<Skill>();
            _mockRepository.Setup(m => m.CheckName(name)).Returns(false);

            //call action
            var result = _skillBusiness.CheckName(name);//true

            //compare
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Save()
        {
            //setup method
            _mockUnitOfWork.Setup(m => m.Commit());

            //call action
            _skillBusiness.Save();

            //compare
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void ValidateName()
        {
            //setup method
            string name = "Skill";
          
            //call action
            var result = _skillBusiness.ValidateName(name);//true

            //compare
            Assert.AreEqual(true, result);
        }

       
    }
}
