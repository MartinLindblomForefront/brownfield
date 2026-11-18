import {
  Alert,
  Anchor,
  Button,
  Container,
  Stack,
  Text,
  Textarea,
  Title,
} from '@mantine/core'
import { useState, type FormEvent } from 'react'
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

type FieldName = 'shortDescription' | 'staffNotes'
type FieldErrors = Partial<Record<FieldName, string>>

const isRecord = (value: unknown): value is Record<string, unknown> =>
  typeof value === 'object' && value !== null

async function getProblem(response: Response) {
  const bodyText = await response.text()
  let body: unknown
  try {
    body = JSON.parse(bodyText) as unknown
  } catch (error) {
    if (!(error instanceof SyntaxError)) {
      throw error
    }
  }

  const fieldErrors: FieldErrors = {}
  if (isRecord(body) && isRecord(body.errors)) {
    for (const field of ['shortDescription', 'staffNotes'] as const) {
      const messages = body.errors[field]
      if (Array.isArray(messages) && typeof messages[0] === 'string') {
        fieldErrors[field] = messages[0]
      }
    }
  }

  const message =
    isRecord(body) && typeof body.title === 'string'
      ? body.title
      : isRecord(body) && typeof body.detail === 'string'
        ? body.detail
        : `Request failed (${response.status}).`

  return { fieldErrors, message }
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
      <Anchor component={Link} to="/accessions/new">
        Register an accession
      </Anchor>
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

function AccessionRegistrationPage() {
  const [shortDescription, setShortDescription] = useState('')
  const [staffNotes, setStaffNotes] = useState('')
  const [fieldErrors, setFieldErrors] = useState<FieldErrors>({})
  const [errorMessage, setErrorMessage] = useState<string | null>(null)
  const [createdId, setCreatedId] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setFieldErrors({})
    setErrorMessage(null)
    setCreatedId(null)

    const trimmedDescription = shortDescription.trim()
    const validationErrors: FieldErrors = {}
    if (!trimmedDescription) {
      validationErrors.shortDescription = 'Enter a short description.'
    } else if (trimmedDescription.length > 200) {
      validationErrors.shortDescription = 'Use 200 characters or fewer.'
    }
    if (staffNotes.length > 2000) {
      validationErrors.staffNotes = 'Use 2,000 characters or fewer.'
    }
    if (Object.keys(validationErrors).length > 0) {
      setFieldErrors(validationErrors)
      return
    }

    setIsSubmitting(true)
    try {
      const response = await fetch('/api/accessions', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          shortDescription: trimmedDescription,
          staffNotes: staffNotes.trim() ? staffNotes : null,
        }),
      })

      if (!response.ok) {
        const problem = await getProblem(response)
        setFieldErrors(problem.fieldErrors)
        if (Object.keys(problem.fieldErrors).length === 0) {
          setErrorMessage(problem.message)
        }
        return
      }

      const result: unknown = await response.json()
      if (!isRecord(result) || typeof result.id !== 'string') {
        setErrorMessage(
          'The accession was created, but the response did not include its identifier.',
        )
        return
      }
      setCreatedId(result.id)
    } catch (error) {
      if (error instanceof TypeError) {
        setErrorMessage(
          'Unable to save the accession. Check your connection and try again.',
        )
      } else if (error instanceof SyntaxError) {
        setErrorMessage(
          'The server returned an invalid response. The accession may have been saved; verify before retrying.',
        )
      } else {
        throw error
      }
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Stack>
      <Title order={1}>Register an accession</Title>
      <form onSubmit={handleSubmit}>
        <Stack>
          <Textarea
            label="Short description"
            description="Briefly identify the material (1–200 characters)."
            placeholder="For example, unidentified meadow grass from collection bag 12."
            required
            maxLength={200}
            value={shortDescription}
            error={fieldErrors.shortDescription}
            onChange={(event) => {
              setShortDescription(event.currentTarget.value)
              setFieldErrors((current) => ({
                ...current,
                shortDescription: undefined,
              }))
              setErrorMessage(null)
            }}
          />
          <Textarea
            label="Staff notes"
            description="Optional; up to 2,000 characters."
            maxLength={2000}
            value={staffNotes}
            error={fieldErrors.staffNotes}
            onChange={(event) => {
              setStaffNotes(event.currentTarget.value)
              setFieldErrors((current) => ({ ...current, staffNotes: undefined }))
              setErrorMessage(null)
            }}
          />
          {errorMessage && (
            <Alert color="red" title="Unable to save accession" role="alert">
              {errorMessage}
            </Alert>
          )}
          {createdId && (
            <Alert color="green" title="Accession registered" role="status">
              Assigned identifier: <strong>{createdId}</strong>
            </Alert>
          )}
          <Button type="submit" loading={isSubmitting}>
            Register accession
          </Button>
          <Anchor component={Link} to="/">
            Return home
          </Anchor>
        </Stack>
      </form>
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
        <Route path="/accessions/new" element={<AccessionRegistrationPage />} />
        <Route path="/about" element={<AboutPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </Container>
  )
}

export default App
