using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using dojoPrep.Models;

namespace dojoPrep.Migrations
{
    [DbContext(typeof(dojoPrepContext))]
    [Migration("20170323223302_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("dojoPrep.Models.Joins", b =>
                {
                    b.Property<int>("JoinId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("StuffId");

                    b.Property<int>("UserId");

                    b.HasKey("JoinId");

                    b.HasIndex("StuffId");

                    b.HasIndex("UserId");

                    b.ToTable("Joins");
                });

            modelBuilder.Entity("dojoPrep.Models.Stuff", b =>
                {
                    b.Property<int>("StuffId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateEnd");

                    b.Property<DateTime>("DateStart");

                    b.Property<string>("Desc");

                    b.Property<TimeSpan>("TimeEnd");

                    b.Property<TimeSpan>("TimeStart");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("StuffId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Stuff");
                });

            modelBuilder.Entity("dojoPrep.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("dojoPrep.Models.Joins", b =>
                {
                    b.HasOne("dojoPrep.Models.Stuff", "Stuff")
                        .WithMany("Events")
                        .HasForeignKey("StuffId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("dojoPrep.Models.User", "User")
                        .WithMany("GoingTo")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("dojoPrep.Models.Stuff", b =>
                {
                    b.HasOne("dojoPrep.Models.User", "Creator")
                        .WithMany("Created")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
