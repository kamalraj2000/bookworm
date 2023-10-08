import './App.css';
import { Container, Row, Col, Form, Pagination } from 'react-bootstrap';
import BookTable from './components/BookTable';
import { useState, useEffect } from 'react';
import { IWork, SearchClient } from './clients/bookworm.client';
import useDebounce from './hooks/debounce';
import SearchBox from './components/SearchBox';

function App() {
  const [books, setBooks] = useState<IWork[]>([]);
  const [searchQuery, setSearchQuery] = useState<string>('The eagle has landed');
  const debouncedSearchQuery = useDebounce(searchQuery, 500);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(0);

  useEffect(() => {
    const searchClient = new SearchClient(process.env.REACT_APP_SEARCH_API_URL);

    function isNullOrWhitespace(input: string | null | undefined): boolean {
      return !input || !input.trim();
    }

    function clearBooks() {
      setBooks([]);
        setTotalPages(1);
        setCurrentPage(1);
    }

    async function loadBooks(query: string)
    {
      if (isNullOrWhitespace(query)) {
        clearBooks();
        return;
      }

      const resultsPerPage = 10;
      const searchResults = await searchClient.searchForWorks(query, (currentPage - 1) * resultsPerPage, resultsPerPage);

      if (searchResults && searchResults.status === 200 && searchResults.result.docs)
      {
        setBooks(searchResults.result.docs);
        const totalPages = Math.ceil((searchResults.result.numFound ?? 0) / resultsPerPage);
        setTotalPages(totalPages);

        if (currentPage > totalPages) {
          setCurrentPage(totalPages);
        }
      }
    }

    loadBooks(debouncedSearchQuery);
  }, [debouncedSearchQuery, currentPage, totalPages]);

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
          {(books.length > 0) &&
            <>
              <BookTable books={books}/>
              <Row className="mt-4">
                <Col className="d-flex justify-content-center">
                  <Pagination>
                    <Pagination.First onClick={() => setCurrentPage(1)} disabled={currentPage === 1} />
                    <Pagination.Prev onClick={() => setCurrentPage(prev => prev - 1)} disabled={currentPage === 1} />
                    <Pagination.Next onClick={() => setCurrentPage(prev => prev + 1)} disabled={currentPage === totalPages} />
                    <Pagination.Last onClick={() => setCurrentPage(totalPages)} disabled={currentPage === totalPages} />
                  </Pagination>
                </Col>
              </Row>
              <Row className="mt-2">
                <Col className="d-flex justify-content-center">
                  <p>Page {currentPage} of {totalPages}</p>
                </Col>
              </Row>
            </>
          }
        </Col>
      </Row>
    </Container>
  );
}

export default App;
