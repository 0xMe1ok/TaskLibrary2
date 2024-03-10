using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Linq;
using System.Numerics;
using TaskLibrary2.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskLibrary2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool formIsLoaded = false;

        int daysOfOrder = 45;

        public void UpdateComboBox(ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
            else
                comboBox.Text = string.Empty;
        }
        public void RefreshClients()
        {
            using (Context context = new Context())
            {
                var clients = (from Client in context.Clients
                               select new
                               {
                                   Client.Id,
                                   Client.Name,
                                   Client.Surname,
                                   Client.Patronymic,
                                   Client.DateOfBirth,
                                   Client.Email
                               }).AsNoTracking();

                dataGridViewMainClients.DataSource = clients.ToList();

                var viewClients = (from Client in context.Clients
                                   select new
                                   {
                                       Идентификатор = Client.Id,
                                       Имя = Client.Name,
                                       Фамилия = Client.Surname,
                                       Отчество = Client.Patronymic,
                                       Дата_рождения = Client.DateOfBirth,
                                       Электронная_почта = Client.Email
                                   }).AsNoTracking();

                dataGridViewClients.DataSource = viewClients.ToList();

                var comboClients = (from Client in context.Clients
                                    select new
                                    {
                                        Идентификатор = Client.Id,
                                        Клиент = Client.Surname + " " + Client.Name + " " + Client.Patronymic + " | " + Client.Email
                                    }).AsNoTracking();

                comboBoxBookOrderClient.DataSource = comboClients.ToList();
                comboBoxBookOrderClient.DisplayMember = "Клиент";
                comboBoxBookOrderClient.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBookOrderClient);
            }
        }

        public void RefreshAuthors()
        {
            using (Context context = new Context())
            {
                var authors = (from Author in context.Authors
                               select new
                               {
                                   Author.Id,
                                   Author.Name,
                                   Author.Surname,
                                   Author.Patronymic,
                                   Author.Country
                               }).AsNoTracking();

                var viewAuthors = (from Author in context.Authors
                                   select new
                                   {
                                       Идентификатор = Author.Id,
                                       Имя = Author.Name,
                                       Фамилия = Author.Surname,
                                       Отчество = Author.Patronymic,
                                       Страна = Author.Country
                                   }).AsNoTracking();

                dataGridViewAuthors.DataSource = viewAuthors.ToList();

                dataGridViewMainAuthors.DataSource = authors.ToList();

                var comboAuthors = (from Author in context.Authors
                                    select new
                                    {
                                        Идентификатор = Author.Id,
                                        Автор = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                    }).AsNoTracking();

                comboBoxBookAuthor.DataSource = comboAuthors.ToList(); // RefreshRawAddingBook
                comboBoxBookAuthor.DisplayMember = "Автор";
                comboBoxBookAuthor.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBookAuthor);
            }
        }

        public void RefreshBooks()
        {
            using (Context context = new Context())
            {
                var viewBooks = (from Book in context.Books
                                 select new
                                 {
                                     Book.Id,
                                     Book.Title,
                                     Book.AuthorId,
                                     Book.LibraryId
                                 }).AsNoTracking();

                dataGridViewMainBooks.DataSource = viewBooks.ToList();

                var comboBooks = (from Book in context.Books
                                  join Author in context.Authors
                                  on Book.AuthorId equals Author.Id
                                  select new
                                  {
                                      Id = Book.Id,
                                      Title = Book.Title,
                                      Author = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                  }
                                  into temp
                                  group temp by temp.Title into table
                                  select new
                                  {
                                      Идентификатор = table.Min(x => x.Id),
                                      Книга = table.Key + " | " + table.Min(x => x.Author),
                                  }).AsNoTracking();

                comboBoxBook_SBookId.DataSource = comboBooks.ToList(); // RefreshSubstAddingBook
                comboBoxBook_SBookId.DisplayMember = "Книга";
                comboBoxBook_SBookId.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBook_SBookId);
            }
        }

        public void RefreshLibrary()
        {
            using (Context context = new Context())
            {
                var library = (from Library in context.Libraries
                               select new
                               {
                                   Library.Id,
                                   Library.Address,
                                   Library.PhoneNumber
                               }).AsNoTracking();

                dataGridViewMainLibrary.DataSource = library.ToList();
            }
        }

        public void RefreshLibraries()
        {
            using (Context context = new Context())
            {
                var libraries = (from Libraries in context.Library_Numbers
                                 select new
                                 {
                                     Libraries.Id,
                                     Libraries.LibraryId
                                 }).AsNoTracking();

                dataGridViewMainLibraries.DataSource = libraries.ToList();
            }
        }

        public void RefreshLibraryToSelectInBooks()
        {
            using (Context context = new Context())
            {
                var viewLibraries = (from Libraries in context.Library_Numbers
                                     join Library in context.Libraries
                                     on Libraries.LibraryId equals Library.Id
                                     select new
                                     {
                                         Идентификатор = Libraries.Id,
                                         Библиотека = Libraries.LibraryId + " | " + Library.Address
                                     }).AsNoTracking();

                comboBoxBook_SelectLibraryNumber.DataSource = viewLibraries.ToList();
                comboBoxBook_SelectLibraryNumber.DisplayMember = "Библиотека";
                comboBoxBook_SelectLibraryNumber.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBook_SelectLibraryNumber);

                comboBoxBookOrder_SearchBook_Library.DataSource = viewLibraries.ToList();
                comboBoxBookOrder_SearchBook_Library.DisplayMember = "Библиотека";
                comboBoxBookOrder_SearchBook_Library.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBookOrder_SearchBook_Library);

            }
        }

        public void RefreshBookOrders()
        {
            using (Context context = new Context())
            {
                var viewMainBookOrders = (from BookOrder in context.BookOrders
                                          select new
                                          {
                                              BookOrder.Id,
                                              BookOrder.ClientId,
                                              BookOrder.BookId,
                                              BookOrder.LibraryId,
                                              BookOrder.DateOfOrder,
                                              BookOrder.DateOfReturn
                                          }).AsNoTracking();

                dataGridViewMainBookOrders.DataSource = viewMainBookOrders.ToList();

                var viewBookOrders = (from BookOrder in context.BookOrders
                                      join Client in context.Clients
                                      on BookOrder.ClientId equals Client.Id
                                      join Book in context.Books
                                      on BookOrder.BookId equals Book.Id
                                      join Author in context.Authors
                                      on Book.AuthorId equals Author.Id
                                      join Libraries in context.Library_Numbers
                                      on BookOrder.LibraryId equals Libraries.Id
                                      join Library in context.Libraries
                                      on Libraries.LibraryId equals Library.Id
                                      select new
                                      {
                                          Идентификатор = BookOrder.Id,
                                          Клиент = Client.Surname + " " + Client.Name + " " + Client.Patronymic + " | " + Client.Email,
                                          Книга = Book.Title + " | " + Author.Name + " " + Author.Patronymic + " " + Author.Surname,
                                          Библиотека = Libraries.LibraryId + " | " + Library.Address,
                                          Дата_записи = BookOrder.DateOfOrder,
                                          Дата_возврата = BookOrder.DateOfReturn
                                      }).AsNoTracking();

                dataGridViewBookOrders.DataSource = viewBookOrders.ToList();
            }
        }

        public void SearchBookOrders(string client)
        {
            using (Context context = new Context())
            {
                var viewBookOrders = (from BookOrder in context.BookOrders
                                      join Client in context.Clients
                                      on BookOrder.ClientId equals Client.Id
                                      where (Client.Surname + " " + Client.Name + " " + Client.Patronymic).Contains(client)
                                      join Book in context.Books
                                      on BookOrder.BookId equals Book.Id
                                      join Author in context.Authors
                                      on Book.AuthorId equals Author.Id
                                      join Libraries in context.Library_Numbers
                                      on BookOrder.LibraryId equals Libraries.Id
                                      join Library in context.Libraries
                                      on Libraries.LibraryId equals Library.Id
                                      select new
                                      {
                                          Идентификатор = BookOrder.Id,
                                          Клиент = Client.Surname + " " + Client.Name + " " + Client.Patronymic + " | " + Client.Email,
                                          Книга = Book.Title + " | " + Author.Name + " " + Author.Patronymic + " " + Author.Surname,
                                          Библиотека = Libraries.LibraryId + " | " + Library.Address,
                                          Дата_записи = BookOrder.DateOfOrder,
                                          Дата_возврата = BookOrder.DateOfReturn
                                      }).AsNoTracking();

                dataGridViewBookOrders.DataSource = viewBookOrders.ToList();
            }
        }

        public void RefreshLibraryWithNumber()
        {
            using (Context context = new Context())
            {
                var viewLibraries = (from Library in context.Libraries
                                     select new
                                     {
                                         Идентификатор = Library.Id,
                                         Адрес = Library.Address,
                                         Номер_телефона = Library.PhoneNumber,
                                     }).AsNoTracking();

                dataGridViewLibraryWithNumber.DataSource = viewLibraries.ToList();

                var comboLibraries = (from Libraries in context.Library_Numbers
                                      join Library in context.Libraries
                                     on Libraries.LibraryId equals Library.Id
                                      select new
                                      {
                                          Идентификатор = Libraries.Id,
                                          Библиотека = Libraries.LibraryId + " | " + Library.Address
                                      }).AsNoTracking();

                comboBoxBook_LibraryNumber.DataSource = comboLibraries.ToList(); // RefreshRawAddingBook
                comboBoxBook_LibraryNumber.DisplayMember = "Библиотека";
                comboBoxBook_LibraryNumber.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBook_LibraryNumber);

                comboBoxBook_SLibraryNumber.DataSource = comboLibraries.ToList(); // RefreshSubstAddingBook
                comboBoxBook_SLibraryNumber.DisplayMember = "Библиотека";
                comboBoxBook_SLibraryNumber.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBook_SLibraryNumber);
            }
        }

        public void RefreshBooksCount()
        {
            using (Context context = new Context())
            {
                var currLibId = Convert.ToInt32(comboBoxBook_SelectLibraryNumber.SelectedValue);

                var viewBooks = (from Book in context.Books
                                 where (Book.LibraryId == currLibId)
                                 join Author in context.Authors
                                 on Book.AuthorId equals Author.Id
                                 where (!(context.BookOrders.Select(c => c.BookId).Contains(Book.Id)))
                                 select new
                                 {
                                     Id = Book.Id,
                                     Title = Book.Title,
                                     Author = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                 }
                                 into temp
                                 group temp by temp.Title into table
                                 select new
                                 {
                                     Идентификатор = table.Min(x => x.Id),
                                     Книга = table.Key + " | " + table.Min(x => x.Author),
                                     Количество = table.Count(x => x.Title.Equals(table.Key))
                                 }).AsNoTracking();

                dataGridViewBooks.DataSource = viewBooks.ToList();
            }
        }

        public void RefreshBooksCount(string bookOrAuthor)
        {
            using (Context context = new Context())
            {
                var currLibId = Convert.ToInt32(comboBoxBook_SelectLibraryNumber.SelectedValue);

                var viewBooks = (from Book in context.Books
                                 where (Book.LibraryId == currLibId)
                                 join Author in context.Authors
                                 on Book.AuthorId equals Author.Id
                                 where (Book.Title.Contains(bookOrAuthor) || (Author.Name + " " + Author.Patronymic + " " + Author.Surname).Contains(bookOrAuthor))
                                 where (!(context.BookOrders.Select(c => c.BookId).ToList().Contains(Book.Id)))
                                 select new
                                 {
                                     Id = Book.Id,
                                     Title = Book.Title,
                                     Author = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                 }
                                 into temp
                                 group temp by temp.Title into table
                                 select new
                                 {
                                     Идентификатор = table.Min(x => x.Id),
                                     Книга = table.Key + " | " + table.Min(x => x.Author),
                                     Количество = table.Count(x => x.Title.Equals(table.Key))
                                 }).AsNoTracking();

                dataGridViewBooks.DataSource = viewBooks.ToList();
            }
        }

        public void RefreshBookOrderSearch()
        {
            using (Context context = new Context())
            {
                var currLibId = Convert.ToInt32(comboBoxBookOrder_SearchBook_Library.SelectedValue);

                var comboBooks = (from Book in context.Books
                                  where (Book.LibraryId == currLibId)
                                  join Author in context.Authors
                                  on Book.AuthorId equals Author.Id
                                  where (!(context.BookOrders.Select(c => c.BookId).ToList().Contains(Book.Id)))
                                  select new
                                  {
                                      Id = Book.Id,
                                      Title = Book.Title,
                                      Author = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                  }
                                  into temp
                                  group temp by temp.Title into table
                                  select new
                                  {
                                      Идентификатор = table.Min(x => x.Id),
                                      Книга = table.Key + " | " + table.Min(x => x.Author),
                                  }).AsNoTracking();

                comboBoxBookOrderAvailableBooks.DataSource = comboBooks.ToList();
                comboBoxBookOrderAvailableBooks.DisplayMember = "Книга";
                comboBoxBookOrderAvailableBooks.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBookOrderAvailableBooks);
            }
        }

        public void RefreshBookOrderSearch(string bookOrAuthor)
        {
            using (Context context = new Context())
            {
                var currLibId = Convert.ToInt32(comboBoxBookOrder_SearchBook_Library.SelectedValue);

                var comboBooks = (from Book in context.Books
                                  where (Book.LibraryId == currLibId)
                                  join Author in context.Authors
                                  on Book.AuthorId equals Author.Id
                                  where (Book.Title.Contains(bookOrAuthor) || (Author.Name + " " + Author.Patronymic + " " + Author.Surname).Contains(bookOrAuthor))
                                  where (!(context.BookOrders.Select(c => c.BookId).ToList().Contains(Book.Id)))
                                  select new
                                  {
                                      Id = Book.Id,
                                      Title = Book.Title,
                                      Author = Author.Name + " " + Author.Patronymic + " " + Author.Surname
                                  }
                                  into temp
                                  group temp by temp.Title into table
                                  select new
                                  {
                                      Идентификатор = table.Min(x => x.Id),
                                      Книга = table.Key + " | " + table.Min(x => x.Author),
                                  }).AsNoTracking();

                comboBoxBookOrderAvailableBooks.DataSource = comboBooks.ToList();
                comboBoxBookOrderAvailableBooks.DisplayMember = "Книга";
                comboBoxBookOrderAvailableBooks.ValueMember = "Идентификатор";
                UpdateComboBox(comboBoxBookOrderAvailableBooks);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshClients();
            RefreshAuthors();
            RefreshBooks();
            RefreshLibrary();
            RefreshLibraries();
            RefreshBookOrders();

            RefreshLibraryToSelectInBooks();
            RefreshLibraryWithNumber();
            RefreshBooksCount();
            RefreshBookOrderSearch();

            dataGridViewClients.Columns["Идентификатор"].Visible = false;
            dataGridViewAuthors.Columns["Идентификатор"].Visible = false;
            dataGridViewBooks.Columns["Идентификатор"].Visible = false;
            dataGridViewBookOrders.Columns["Идентификатор"].Visible = false;

            dataGridViewMainAuthors.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewMainBooks.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewMainClients.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewMainBookOrders.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewMainLibrary.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewMainLibraries.DefaultCellStyle.ForeColor = Color.Black;

            dataGridViewAuthors.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewClients.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewLibraryWithNumber.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewBooks.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewBookOrders.DefaultCellStyle.ForeColor = Color.Black;

            formIsLoaded = true;
        }

        private void buttonAddClient_Click(object sender, EventArgs e)
        {
            if (textBoxClientName.Text != string.Empty && 
                textBoxClientSurname.Text != string.Empty &&
                textBoxClientPatronymic.Text != string.Empty && 
                textBoxClientEmail.Text != string.Empty)
            {
                using (Context context = new Context())
                {
                    Client client = new Client();
                    client.Name = textBoxClientName.Text;
                    client.Surname = textBoxClientSurname.Text;
                    client.Patronymic = textBoxClientPatronymic.Text;
                    client.Email = textBoxClientEmail.Text;
                    client.DateOfBirth = dateTimePickerClientDateOfBirth.Value.Date;
                    if (context.Clients.Count() > 0)
                        client.Id = context.Clients.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;

                    if (!context.Clients.Any(o => (o.Name == client.Name && 
                                               o.Surname == client.Surname &&
                                               o.Patronymic == client.Patronymic &&
                                               o.DateOfBirth == client.DateOfBirth) ||
                                               o.Email == client.Email))
                    {
                        context.Clients.Add(client);
                        context.SaveChanges();

                        RefreshClients();
                        MessageBox.Show("Клиент был добавлен!");
                    }
                    else
                    {
                        MessageBox.Show("Данные клиента (или почта) уже имеются в БД!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Заполните данные клиента корректно!");
            }
        }

        private void buttonDeleteClients_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count > 0)
            {
                using (Context context = new Context())
                {
                    List<Client> clients = new List<Client>();

                    foreach (DataGridViewRow row in dataGridViewClients.SelectedRows)
                    {
                        var toBeDeleted = (int)row.Cells[0].Value;
                        clients.Add(context.Clients.First(c => c.Id == toBeDeleted));
                    }

                    context.Clients.RemoveRange(clients);
                    context.SaveChanges();

                    RefreshClients();
                    MessageBox.Show("Успешно удалены выбранные клиенты!");
                }
            }
            else
            {
                MessageBox.Show("Не было выбрано клиентов для удаления!");
            }
        }

        private void buttonAddAuthor_Click(object sender, EventArgs e)
        {
            if (textBoxAuthorName.Text != string.Empty && 
                textBoxAuthorSurname.Text != string.Empty &&
                textBoxAuthorPatronymic.Text != string.Empty && 
                textBoxAuthorCountry.Text != string.Empty)
            {
                using (Context context = new Context())
                {
                    Author author = new Author();
                    author.Name = textBoxAuthorName.Text;
                    author.Surname = textBoxAuthorSurname.Text;
                    author.Patronymic = textBoxAuthorPatronymic.Text;
                    author.Country = textBoxAuthorCountry.Text;
                    if (context.Authors.Count() > 0)
                        author.Id = context.Authors.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;

                    if (!context.Authors.Any(o => o.Name == author.Name &&
                                                   o.Surname == author.Surname &&
                                                   o.Patronymic == author.Patronymic &&
                                                   o.Country == author.Country))
                    {
                        context.Authors.Add(author);
                        context.SaveChanges();

                        RefreshAuthors();
                        MessageBox.Show("Автор был добавлен!");
                    }
                    else
                    {
                        MessageBox.Show("Данные автора уже имеются в БД!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Заполните данные автора корректно!");
            }
        }

        private void buttonDeleteAuthors_Click(object sender, EventArgs e)
        {
            if (dataGridViewAuthors.SelectedRows.Count > 0)
            {
                using (Context context = new Context())
                {
                    List<Author> authors = new List<Author>();

                    foreach (DataGridViewRow row in dataGridViewAuthors.SelectedRows)
                    {
                        var toBeDeleted = (int)row.Cells[0].Value;
                        authors.Add(context.Authors.First(c => c.Id == toBeDeleted));
                    }

                    context.Authors.RemoveRange(authors);
                    context.SaveChanges();

                    RefreshAuthors();


                    MessageBox.Show("Успешно удалены выбранные авторы!");
                }
            }
            else
            {
                MessageBox.Show("Не было выбрано авторов для удаления!");
            }
        }

        private void buttonAddLibrary_Click(object sender, EventArgs e)
        {
            if (textBoxLibraryAddress.Text != string.Empty && 
                textBoxLibraryPhoneNumber.Text != string.Empty)
            {
                using (Context context = new Context())
                {
                    Library library = new Library();
                    library.Id = (int)numericUpDownLibraryNumber.Value;
                    library.Address = textBoxLibraryAddress.Text;
                    library.PhoneNumber = textBoxLibraryPhoneNumber.Text;

                    Libraries libNumber = new Libraries();
                    libNumber.LibraryId = (int)numericUpDownLibraryNumber.Value;
                    if (context.Libraries.Count() > 0)
                        libNumber.Id = context.Books.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;

                    if (!context.Libraries.Any(o => o.Id == library.Id ||
                                                    o.Address == library.Address &&
                                                    o.PhoneNumber == library.PhoneNumber))
                    {
                        context.Libraries.Add(library);
                        context.Library_Numbers.Add(libNumber);
                        context.SaveChanges();

                        RefreshLibrary();
                        RefreshLibraries();
                        RefreshLibraryWithNumber();
                        RefreshLibraryToSelectInBooks();

                        MessageBox.Show("Библиотека была добавлена!");
                    }
                    else
                    {
                        MessageBox.Show("Данные библиотеки (или номер) уже имеются в БД!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Заполните данные библиотеки корректно!");
            }
        }

        private void buttonDeleteSelectedLibraries_Click(object sender, EventArgs e)
        {
            if (dataGridViewLibraryWithNumber.SelectedRows.Count > 0)
            {
                using (Context context = new Context())
                {
                    List<Library> libraries = new List<Library>();
                    List<Libraries> library_numbers = new List<Libraries>();

                    using var transaction = context.Database.BeginTransaction();

                    foreach (DataGridViewRow row in dataGridViewLibraryWithNumber.SelectedRows)
                    {
                        var toBeDeleted = (int)row.Cells[0].Value;
                        libraries.Add(context.Libraries.First(c => c.Id == toBeDeleted));
                        library_numbers.Add(context.Library_Numbers.First(c => c.LibraryId == toBeDeleted));
                    }

                    context.Libraries.RemoveRange(libraries);
                    context.Library_Numbers.RemoveRange(library_numbers);
                    context.SaveChanges();

                    transaction.Commit();

                    RefreshLibrary();
                    RefreshLibraries();
                    RefreshLibraryWithNumber();

                    MessageBox.Show("Успешно удалены выбранные библиотеки!");
                }
            }
            else
            {
                MessageBox.Show("Не было выбрано библиотек для удаления!");
            }
        }

        private void buttonAddRawBook_Click(object sender, EventArgs e)
        {
            if (textBoxBookTitle.Text != string.Empty && 
                comboBoxBookAuthor.SelectedIndex != -1 && 
                comboBoxBook_LibraryNumber.SelectedIndex != -1)
            {
                using (Context context = new Context())
                {
                    Book book = new Book();
                    book.Title = textBoxBookTitle.Text;
                    book.AuthorId = Convert.ToInt32(comboBoxBookAuthor.SelectedValue);
                    book.LibraryId = Convert.ToInt32(comboBoxBook_LibraryNumber.SelectedValue);
                    if (context.Books.Count() > 0)
                        book.Id = context.Books.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;

                    context.Books.Add(book);
                    context.SaveChanges();

                    RefreshBooks();
                    RefreshBooksCount();
                    RefreshBookOrderSearch();

                    MessageBox.Show("Книга была добавлена!");
                }

            }
            else
            {
                MessageBox.Show("Заполните данные книги корректно!");
            }
        }

        private void buttonAddSubstBook_Click(object sender, EventArgs e)
        {
            using (Context context = new Context())
            {
                if (comboBoxBook_SBookId.SelectedIndex != -1 && 
                    comboBoxBook_SLibraryNumber.SelectedIndex != -1)
                {
                    Book selectedBook = context.Books.First(c => c.Id == Convert.ToInt32(comboBoxBook_SBookId.SelectedValue));
                    int bookCount = (int)numericUpDownBook_SCount.Value;
                    List<Book> books = new List<Book>();
                    int lastId = context.Books.OrderByDescending(c => c.Id).FirstOrDefault().Id;

                    for (int i = 0; i < bookCount; i++)
                    {
                        Book book = new Book();
                        book.Title = selectedBook.Title;
                        book.AuthorId = selectedBook.AuthorId;
                        book.LibraryId = Convert.ToInt32(comboBoxBook_SLibraryNumber.SelectedValue);
                        book.Id = lastId + i + 1;
                        books.Add(book);
                    }

                    context.Books.AddRange(books);
                    context.SaveChanges();

                    RefreshBooks();
                    RefreshBooksCount();
                    RefreshBookOrderSearch();

                    if (bookCount > 1)
                    {
                        MessageBox.Show("Книги были добавлены!");
                    }
                    else 
                    {
                        MessageBox.Show("Книга была добавлена!");
                    }
                }
                else
                {
                    MessageBox.Show("Заполните данные книги корректно!");
                }
            }
        }

        private void buttonDeleteBooks_Click(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count > 0) 
            { 
                using (Context context = new Context())
                {
                    var count = (int)numericUpDownBookCountToDelete.Value;
                    var iterations = 1;

                    using var transaction = context.Database.BeginTransaction();

                    foreach (DataGridViewRow row in dataGridViewBooks.SelectedRows)
                    {
                        var toBeDeleted = (int)row.Cells[0].Value; // Идентификатор

                        var selectedBook = (string)row.Cells[1].Value; // Название книги
                        var selectedBookCount = (int)row.Cells[2].Value; // Количество

                        var libNumber = Convert.ToInt32(comboBoxBook_SelectLibraryNumber.SelectedValue);

                        if (selectedBookCount >= count)
                        {
                            var book = context.Books.First(c => c.Id == toBeDeleted);

                            var title = book.Title;
                            var authorId = book.AuthorId;

                            var excludedIDs = new HashSet<int>(context.BookOrders.Select(c => c.Id));
                            var books = context.Books.Where(c => (!excludedIDs.Contains(c.Id)) && c.Title == title && c.AuthorId == authorId && c.LibraryId == libNumber).Take(count);

                            context.Books.RemoveRange(books);
                            context.SaveChanges();

                            if (checkBoxBook_MessageOnEveryBook.Checked)
                            {
                                MessageBox.Show($"{iterations} / {dataGridViewBooks.SelectedRows.Count} | Удалено {count} {DeclensionGenerator.Generate(count, "книга", "книги", "книг")} {selectedBook}");
                                iterations++;
                            }
                        }
                        else
                        {
                            if (checkBoxBook_MessageOnEveryBook.Checked)
                            {
                                MessageBox.Show($"{iterations} / {dataGridViewBooks.SelectedRows.Count} | Не удалось удалить {count} {DeclensionGenerator.Generate(count, "книга", "книги", "книг")} {selectedBook}");
                                iterations++;
                            }
                        }
                    }

                    transaction.Commit();

                    RefreshBooks();
                    RefreshBooksCount();

                    if (checkBoxBook_MessageOnEveryBook.Checked)
                    {
                        MessageBox.Show("Удаление книг закончено!");
                    }
                    else
                    {
                        MessageBox.Show("Успешно произведены действия по удалению выбранных книг!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Не было выбрано книг для удаления!");
            }
        }

        private void buttonBookSearch_Click(object sender, EventArgs e)
        {
            if (textBoxBookSearch_TitleOrAuthor.Text != string.Empty)
            {
                RefreshBooksCount(textBoxBookSearch_TitleOrAuthor.Text);
            }
            else
            {
                RefreshBooksCount();
            }
        }

        private void buttonResetSearch_Click(object sender, EventArgs e)
        {
            RefreshBooksCount();
        }

        private void comboBoxBook_SelectLibraryNumber_SelectedValueChanged(object sender, EventArgs e)
        {
            if (formIsLoaded)
            {
                RefreshBooksCount();
            }
        }

        private void buttonBookOrder_SearchBook_Search_Click(object sender, EventArgs e)
        {
            if (textBoxBookOrder_SearchBook_TitleOrAuthor.Text != string.Empty)
            {
                RefreshBookOrderSearch(textBoxBookOrder_SearchBook_TitleOrAuthor.Text);
            }
            else
            {
                RefreshBookOrderSearch();
            }
        }

        private void buttonBookOrder_SearchBook_ResetSearch_Click(object sender, EventArgs e)
        {
            RefreshBookOrderSearch();
        }

        private void comboBoxBookOrder_SearchBook_Library_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formIsLoaded)
            {
                RefreshBookOrderSearch();
            }
        }

        private void buttonBookOrder_AddOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxBookOrderClient.SelectedIndex != -1 && 
                comboBoxBookOrder_SearchBook_Library.SelectedIndex != -1 && 
                comboBoxBookOrderAvailableBooks.SelectedIndex != -1)
            {
                using (Context context = new Context())
                {
                    BookOrder bookOrder = new BookOrder();
                    bookOrder.ClientId = Convert.ToInt32(comboBoxBookOrderClient.SelectedValue);
                    bookOrder.BookId = Convert.ToInt32(comboBoxBookOrderAvailableBooks.SelectedValue);
                    bookOrder.LibraryId = Convert.ToInt32(comboBoxBookOrder_SearchBook_Library.SelectedValue);
                    bookOrder.DateOfOrder = DateTime.Now;
                    bookOrder.DateOfReturn = DateTime.Now.AddDays(daysOfOrder);

                    if (!context.BookOrders.Any(o => o.ClientId == bookOrder.ClientId &&
                                                   o.BookId == bookOrder.BookId))
                    {
                        context.BookOrders.Add(bookOrder);
                        context.SaveChanges();

                        RefreshBooks();
                        RefreshBooksCount();
                        RefreshBookOrderSearch();
                        RefreshBookOrders();
                        MessageBox.Show("Запись была добавлена!");
                    }
                    else
                    {
                        MessageBox.Show("Данный клиент уже брал выбранную книгу!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните данные записи корректно!");
            }
        }

        private void buttonBookOrder_ReturnBooks_Click(object sender, EventArgs e)
        {
            if (dataGridViewBookOrders.SelectedRows.Count > 0)
            {
                using (Context context = new Context())
                {
                    List<BookOrder> bookOrders = new List<BookOrder>();

                    foreach (DataGridViewRow row in dataGridViewBookOrders.SelectedRows)
                    {
                        var toBeDeleted = (int)row.Cells[0].Value;
                        bookOrders.Add(context.BookOrders.First(c => c.Id == toBeDeleted));
                    }

                    context.BookOrders.RemoveRange(bookOrders);
                    context.SaveChanges();

                    RefreshBooks();
                    RefreshBooksCount();
                    RefreshBookOrderSearch();
                    RefreshBookOrders();

                    MessageBox.Show("Успешно удалены выбранные записи!");
                }
            }
            else
            {
                MessageBox.Show("Не было выбрано записей книг для их удаления!");
            }
        }

        private void buttonBookOrder_SearchClient_Click(object sender, EventArgs e)
        {
            if (textBoxBookOrder_SearchClient.Text != string.Empty)
            {
                SearchBookOrders(textBoxBookOrder_SearchClient.Text);
            }
            else
            {
                RefreshBookOrders();
            }
        }

        private void buttonBookOrder_ResetSearchClient_Click(object sender, EventArgs e)
        {
            RefreshBookOrders();
        }

        
    }
}
