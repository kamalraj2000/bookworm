import React from 'react';
import PropTypes from 'prop-types';
import { Table, Button } from 'react-bootstrap';

const books = [
  {
    title: "To Kill a Mockingbird",
    author: "Harper Lee",
    link: "https://example.com/to-kill-a-mockingbird"
  },
  {
    title: "1984",
    author: "George Orwell",
    link: "https://example.com/1984"
  },
  // ... add more books as needed
];

const BookTable = () => {
  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>Book Title</th>
          <th>Author</th>
          <th>Link to Book</th>
        </tr>
      </thead>
      <tbody>
        {books.map((book, index) => (
          <tr key={index}>
            <td>{book.title}</td>
            <td>{book.author}</td>
            <td>
              <Button variant="link" href={book.link} target="_blank" rel="noopener noreferrer">
                View Book
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
}

// BookTable.propTypes = {
//   books: PropTypes.array.isRequired
// };

export default BookTable;
