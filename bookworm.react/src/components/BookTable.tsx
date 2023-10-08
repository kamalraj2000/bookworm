import React from 'react';
import { Table, Button } from 'react-bootstrap';
import { IWork } from '../clients/bookworm.client';

type BookTableProps = {
  books: IWork[]
}

const BookTable = ({books}: BookTableProps) => {
  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>Book Title</th>
          <th>Author</th>
          <th>Link to Open Library Page</th>
        </tr>
      </thead>
      <tbody>
        {books.map((book, index) => (
          <tr key={index}>
            <td>{book.title}</td>
            <td>{book.author_name?.[0] ?? ""}</td>
            <td>
              <Button variant="link" href={"https://openlibrary.org/" + book.key} target="_blank" rel="noopener noreferrer">
                Go to Book Page
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
}

export default BookTable;
