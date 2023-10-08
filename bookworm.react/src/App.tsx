import './App.css';
import { Container, Row, Col, Form } from 'react-bootstrap';
import BookTable from './components/BookTable';
import { useState, useEffect } from 'react';
import { IWork, SearchClient } from './clients/bookworm.client';
import useDebounce from './hooks/debounce';

function App() {
  const [books, setBooks] = useState<IWork[]>([]);
  const [searchQuery, setSearchQuery] = useState<string>('The eagle has landed');
  const debouncedSearchQuery = useDebounce(searchQuery, 500);

  useEffect(() => {
    const searchClient = new SearchClient(process.env.REACT_APP_SEARCH_API_URL);

    async function loadBooks(query: string)
    {
      const searchResults = await searchClient.searchForWorks(query, 0, 10);
      if (searchResults && searchResults.status == 200 && searchResults.result.docs)
      {
        setBooks(searchResults.result.docs);
      }
    }

    loadBooks(debouncedSearchQuery);
  }, [debouncedSearchQuery]);

  return (
    <Container className="mt-5">
      <Row>
        <Col>
          <h1 className="mb-4">Book List</h1>
          <Form.Group className="mb-4">
            <Form.Label>Enter Book Name</Form.Label>
            <Form.Control 
                type="text" 
                placeholder="Search for a book..." 
                value={searchQuery} 
                onChange={e => setSearchQuery(e.target.value)}
            />
          </Form.Group>
          <BookTable books={books}/>
        </Col>
      </Row>
    </Container>
  );
}

export default App;
