import './App.css';
import { Container, Row, Col, Form, InputGroup } from 'react-bootstrap';
import BookTable from './components/BookTable';
import { useState, useEffect } from 'react';
import { IWork, SearchClient } from './clients/bookworm.client';
import useDebounce from './hooks/debounce';
import SearchBox from './components/SearchBox';

function App() {
  const [books, setBooks] = useState<IWork[]>([]);
  const [searchQuery, setSearchQuery] = useState<string>('The eagle has landed');
  const debouncedSearchQuery = useDebounce(searchQuery, 500);

  useEffect(() => {
    const searchClient = new SearchClient(process.env.REACT_APP_SEARCH_API_URL);

    function isNullOrWhitespace(input: string | null | undefined): boolean {
      return !input || !input.trim();
    }

    async function loadBooks(query: string)
    {
      if (isNullOrWhitespace(query)) {
        setBooks([]);
        return;
      }

      const searchResults = await searchClient.searchForWorks(query, 0, 10);
      if (searchResults && searchResults.status === 200 && searchResults.result.docs)
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
        <Form.Group className="mb-4" controlId="bookSearch">
            <Row>
              <Col xs={3} className="align-self-center">
                <Form.Label>Enter Book Name</Form.Label>
              </Col>
              <Col>
                <SearchBox bookTitle={searchQuery} onChange={e => setSearchQuery(e.target.value)} />
              </Col>
            </Row>
          </Form.Group>
          <BookTable books={books}/>
        </Col>
      </Row>
    </Container>
  );
}

export default App;
