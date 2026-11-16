import { Anchor, Container, Stack, Text, Title } from '@mantine/core'
import { Link, Route, Routes } from 'react-router-dom'
import useSWR from 'swr'

type HealthResponse = {
  status: string
}

const fetcher = async (url: string): Promise<HealthResponse> => {
  const response = await fetch(url)
  if (!response.ok) {
    throw new Error(`Request failed (${response.status})`)
  }
  return response.json() as Promise<HealthResponse>
}

function HomePage() {
  const { data, error, isLoading } = useSWR<HealthResponse>(
    '/api/health',
    fetcher,
  )
  const backendStatus = error
    ? `Backend unavailable: ${error.message}`
    : isLoading || !data
      ? 'Checking backend...'
      : `Backend status: ${data.status}`

  return (
    <Stack>
      <Title order={1}>Accession Registry</Title>
      <Text>This is the starting point for the accession registration app.</Text>
      <Text role="status">{backendStatus}</Text>
      <Anchor component={Link} to="/about">
        About this starter
      </Anchor>
    </Stack>
  )
}

function AboutPage() {
  return (
    <Stack>
      <Title order={1}>Application foundation</Title>
      <Text>The client is built with React, TypeScript, and Vite.</Text>
      <Anchor component={Link} to="/">
        Home
      </Anchor>
    </Stack>
  )
}

function NotFoundPage() {
  return <Title order={1}>Page not found</Title>
}

function App() {
  return (
    <Container size="md" py="xl">
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/about" element={<AboutPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </Container>
  )
}

export default App
