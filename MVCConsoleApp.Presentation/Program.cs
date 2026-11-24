using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data.Entities;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Welcome to Music Manager!");
Console.WriteLine("Press 1 to Add a new record");
Console.WriteLine("Press 2 to View all records ");
Console.WriteLine("Press 3 to View by Role ");
Console.WriteLine("Press 4 to Edit an existing record by Id ");
Console.WriteLine("Press 5 to Delete an existing record by Id ");
Console.WriteLine("Press 8 to Exit ");



try
{
    //Console.WriteLine("Bạn vừa nhập số: " + number);
    int number = 0;
    do
    {
        Console.Write("Please press a number: ");
        number = int.Parse(Console.ReadLine());
        var personRoleController = new PersonRoleController();


        switch (number)
        {
            case 1:
                {


                    Console.WriteLine("Vui lòng nhập tên: ");
                    string name = Console.ReadLine();

                    Console.WriteLine("Vui lòng nhập mô tả: ");
                    string description = Console.ReadLine();


                    bool isCreated = personRoleController.Create(name, description);

                    if (isCreated)
                    {
                        Console.WriteLine("Tạo thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Tạo thất bại!");
                    }
                    break;
                }
            case 2:
                {
                    var roles = personRoleController.GetAll();
                    foreach (var role in roles)
                    {
                        role.Display(role);
                    }

                    break;
                }
            case 3:
                {
                    Console.Write("Vui lòng nhập ID: ");
                    int id = int.Parse(Console.ReadLine());
                    if (id > 0)
                    {
                        var role = personRoleController.GetById(id);
                        if (role != null)
                        {
                            role.Display(role);
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy role!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    break;
                }
            case 4:
                {


                    Console.WriteLine("Vui lòng nhập Id: ");
                    int idToUpdate = int.Parse(Console.ReadLine());
                    if (idToUpdate <= 0)
                    {
                        Console.WriteLine("ID không hợp lệ!");
                        break;
                    }

                    Console.WriteLine("Vui lòng nhập tên: ");
                    string name = Console.ReadLine();

                    Console.WriteLine("Vui lòng nhập mô tả: ");
                    string description = Console.ReadLine();


                    bool isUpdated = personRoleController.Update(idToUpdate, name, description);

                    if (isUpdated)
                    {
                        Console.WriteLine("Cập nhật thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Tạo thất bại!");
                    }
                    break;
                }
            case 5:
                {
                    Console.WriteLine("Nhập Id cần xóa: ");
                    int idToDelete = int.Parse(Console.ReadLine());
                    if (idToDelete <= 0)
                    {
                        Console.WriteLine("ID không hợp lệ!");
                        break;
                    }

                    bool isDeleted = personRoleController.Delete(idToDelete);

                    if (isDeleted)
                    {
                        Console.WriteLine("Xóa thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Xóa thất bại!");
                    }
                    break;
                }
            default:
                break;
        }
    } while (number != 8);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}